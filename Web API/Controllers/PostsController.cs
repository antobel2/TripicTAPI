using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.Models;

namespace Web_API.Controllers
{
    // [Authorize]
    public class PostsController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        
        // GET api/values
        //Retourne la liste des posts de l'utilisateur
        [HttpGet]
        [Route("api/Posts")]
        public IEnumerable<PostDTO> GetPosts()
        {
            //if (db.Posts.Count() == 0)
            //    return null;
            List<PostDTO> postsDTO = new List<PostDTO>();
            foreach (Post post in db.Posts.ToList())
            {
                var pictures = new List<PictureDTO>();
                foreach (Picture picture in post.Pictures)
                {
                    PictureDTO pi = new PictureDTO();
                    pi.Base64 = picture.Base64;
                    //pi.Id = picture.Id;
                    pictures.Add(pi);
                }
                postsDTO.Add(new PostDTO()
                {
                    Id = post.Id,
                    PicturesDTO = pictures,
                    Text = post.Text,
                    //TODO: Changer le user et l'activity
                    //UserId = int.Parse(post.User.Id),
                    //ActivityId = post.Activity.Id
                });
            }
            return postsDTO;
        }
        
        // POST api/values
        //Crée un post, y ajoute le texte s'il y en a, renvoie une réponse et le id si le post est bien créé
        [HttpPost]
        [Route("api/Post/CreatePost")]
        public HttpResponseMessage CreatePost([FromBody]CreatePostDTO value)
        {
            if (!ModelState.IsValid)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);

            Post po = new Post();

            if (value.Text != null && value.Text.Trim() != "" && value.Text.Length <= 250)
                po.Text = value.Text;

            if (value.PictureNumber != 0)
                po.IsValid = false;
            else
                po.IsValid = true;

            //TODO: Changer le user et l'activity
            po.User = db.Users.Find(1);
            po.Activity = db.Activities.Find(1);
            db.Posts.Add(po);
            db.SaveChanges();
            return Request.CreateResponse(po.Id);
        }

        //// PUT api/values/5
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        //// DELETE api/values/5
        //public void Delete(int id)
        //{
        //}

        [Route("api/Posts/GetPostsForActivity")]
        [HttpGet]
        [ResponseType(typeof(List<PostDTO>))]
        public HttpResponseMessage GetPostsForActivity(int id)
        {
            Activity activity = db.Activities.Find(id);
            if (activity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The activity was not found");
            }

            List<PostDTO> results = new List<PostDTO>();
            foreach (Post postInActivity in activity.Posts)
            {
                //transformer post en postDTO
                PostDTO currentPost = new PostDTO
                {
                    Id = postInActivity.Id,
                    Text = postInActivity.Text,
                    PicturesDTO = new List<PictureDTO>()
                };
                //ajouter toutes les photos du post à la liste de photo du dto
                foreach (Picture picturesInPost in postInActivity.Pictures)
                {
                    currentPost.PicturesDTO.Add(new PictureDTO
                    {
                        Id = picturesInPost.Id,
                        Base64 = picturesInPost.Base64
                    });
                }
                results.Add(currentPost);
            }

            return Request.CreateResponse(results);
        }
    }
}
