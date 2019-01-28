using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
   // [Authorize]
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //static List<Post> posts = new List<Post> { new Post("First post"), new Post("Second post") };
        // GET api/values
        //Retourne la liste des posts de l'utilisateur
        [HttpGet]
        [Route("api/Posts")]
        public IEnumerable<PostDTO> GetPosts()
        {
            List<PostDTO> postsDTO = new List<PostDTO>();
            foreach (Post post in db.Posts.ToList())
            {
                postsDTO.Add(new PostDTO()
                {
                    Id = post.Id,
                    Pictures = post.Pictures,
                    Text = post.Text
                });
            }
            return postsDTO;
        }

        //// GET api/values/5
        //[Route("api/values/{id}")]
        //public IEnumerable<Post> GetValues(int id)
        //{
        //}

        // POST api/values
        //Crée un post
        [HttpPost]
        [Route("api/Post/CreatePost")]
        public String Post([FromBody]CreatePostDTO value)
        {
            Post po;
            if (ModelState.IsValid)
            {
                if (value.Text != null)
                {
                    po = new Post(value.Text);

                }
                else
                {
                    po = new Post();
                }

                po.Pictures = value.Pictures;
                db.Posts.Add(po);
                db.SaveChanges();
                return po.Id.ToString();
            }


            return null;
        }


        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE api/values/5
        public void Delete(int id)
        {
        }
    }
}
