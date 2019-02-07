using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.DAL;
using Web_API.DAL.Services;
using Web_API.Models;

namespace Web_API.Controllers
{
    // [Authorize]
    public class PostsController : ApiController
    {
        //private ApplicationDbContext db = new ApplicationDbContext();
        private IServicePosts servicePost = new ServicePosts();
        private UnitOfWork uow = new UnitOfWork();

        // GET api/values
        //Retourne la liste des posts de l'utilisateur
        [HttpGet]
        [Route("api/Posts")]
        public IEnumerable<PostDTO> GetPosts()
        {
            List<PostDTO> postsDTO = new List<PostDTO>();
            foreach (Post post in uow.PostRepository.Get().ToList())
            {
                if (post.IsValid == true)
                {
                    var pictures = new List<PictureDTO>();
                    foreach (Picture picture in post.Pictures)
                    {
                        PictureDTO pi = new PictureDTO();
                        pi.Base64 = picture.Base64;
                        pi.Id = picture.Id;
                        pictures.Add(pi);
                    }
                    postsDTO.Add(servicePost.ToPostDTO(post));
                }
                
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

            if (value.Text != null && value.Text.Trim() != "")
                po.Text = value.Text;

            //Set la propriété IsValid selon le nombre de photos pour valider le post après l'ajout dans la bd
            //(le laisse à true si le post ne contient pas de photos)
            po.PicNumber = value.PicCount;
            if (po.PicNumber != 0)
                po.IsValid = false;
            else
                po.IsValid = true;

            //TODO: Changer le user et l'activity
            //po.User = db.Users.Find(value.UserId);
            //po.Activity = db.Activities.Find(value.ActivityId);
            
            try
            {
                servicePost.CreatePost(po);
            }
            catch (DbEntityValidationException ex)
            {
                foreach (var entityValidationErrors in ex.EntityValidationErrors)
                {
                    foreach (var validationError in entityValidationErrors.ValidationErrors)
                    {
                        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Property: " + validationError.PropertyName + " Error: " + validationError.ErrorMessage);
                    }
                }
            }
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
            Activity activity = uow.ActivityRepository.GetByID(id);
            if (activity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "The activity was not found");
            }

            List<Post> postsInActivity = servicePost.GetPostsForActivity(activity);

            //Transformer les posts en PostDTO
            List<PostDTO> results = new List<PostDTO>();
            foreach (Post post in postsInActivity)
            {
                results.Add(servicePost.ToPostDTO(post));
            }

            return Request.CreateResponse(results);
        }
    }
}
