using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.DAL;
using Web_API.Models;

namespace Web_API.Controllers
{
    [Authorize]
    [RoutePrefix("api/Comments")]
    public class CommentsController : ApiController
    {
        private UnitOfWork uow = new UnitOfWork();

        [HttpGet]
        [Route("GetCommentsByPostId/{id}")]
        public HttpResponseMessage GetCommentsByPostId(int id)
        {
            Post currentPost = uow.PostRepository.GetByID(id);
            List<CommentDTO> results = new List<CommentDTO>();
            foreach (Comment c in currentPost.Comments.OrderBy(x => x.Date))
            {
                CommentDTO comment = new CommentDTO(c);
                results.Add(comment);
            }
            return Request.CreateResponse(results);
        }

        [HttpPost]
        [Route("CreateComment")]
        public HttpResponseMessage CreateComment([FromBody]CreateCommentDTO value)
        {
            if (!ModelState.IsValid)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
            }

            Post currentPost = uow.PostRepository.GetByID(value.PostId);
            if (currentPost == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "L'id du post ne correspond à aucun post");
            }

            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);
            if (currentUser.Trips.FirstOrDefault(x => x.Id == currentPost.Activity.Trip.Id) == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.Forbidden, "L'utilisateur n'a pas accès à la publication");
            }

            Comment comment = new Comment
            {
                Date = DateTime.Now,
                Post = uow.PostRepository.GetByID(value.PostId),
                Text = value.Text,
                User = currentUser
            };

            uow.CommentRepository.Insert(comment);

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}