using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
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
        private UnitOfWork uow = new UnitOfWork();
        private IServicePosts servicePost;
        //private ApplicationUserManager userManager;

        public PostsController()
        {
            this.servicePost = new ServicePosts(uow);
        }
        //public ApplicationUserManager UserManager
        //{
        //    get
        //    {
        //        return userManager ?? Request.GetOwinContext().GetUserManager<ApplicationUserManager>();
        //    }
        //    private set
        //    {
        //        userManager = value;
        //    }
        //}



        // GET api/values
        //Retourne la liste des posts de l'utilisateur
        [HttpGet]
        [Authorize]
        [Route("api/Posts")]
        public IEnumerable<PostDTO> GetPosts()
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);

            List<PostDTO> postsDTO = new List<PostDTO>();
            foreach (Post post in uow.PostRepository.Get().ToList().Where(x => x.User.Id == currentUser.Id))
            {
                if (post.IsValid == true)
                {
                    PostDTO dto = servicePost.ToPostDTO(post);
                    SeenPosts seenStatus = currentUser.SeenPosts.Find(x => x.PostId == post.Id && x.UserId == currentUser.Id);
                    dto.Seen = seenStatus.Seen;
                    postsDTO.Add(servicePost.ToPostDTO(post));
                }

            }
            return postsDTO;
        }

        // POST api/values
        //Crée un post, y ajoute le texte s'il y en a, renvoie une réponse et le id si le post est bien créé
        [HttpPost]
        [Authorize]
        [Route("api/Posts/CreatePost")]
        public HttpResponseMessage CreatePost([FromBody]CreatePostDTO value)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);

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
            po.User = currentUser;
            po.Activity = uow.ActivityRepository.GetByID(value.ActivityId);

            try
            {
                servicePost.CreatePost(po);
                currentUser.Posts.Add(po);
                SeenPosts seen = new SeenPosts
                {
                    Seen = true,
                    Post = po,
                    User = currentUser
                };
                currentUser.SeenPosts.Add(seen);
                uow.Save();
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

        [Route("api/Posts/GetPostsForActivity/{id}")]
        [HttpGet]
        [Authorize]
        [ResponseType(typeof(List<PostDTO>))]
        public HttpResponseMessage GetPostsForActivity(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);


            Activity activity = uow.ActivityRepository.GetByID(id);
            if (activity == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'activité n'a pas été trouvée");
            }

            List<Post> postsInActivity = servicePost.GetPostsForActivity(activity).OrderByDescending(x => x.Date).ToList();

            //Transformer les posts en PostDTO
            List<PostDTO> results = new List<PostDTO>();
            foreach (Post post in postsInActivity)
            {
                PostDTO dto = servicePost.ToPostDTO(post);
                SeenPosts seenStatus = currentUser.SeenPosts.Find(x => x.PostId == post.Id && x.UserId == currentUser.Id);
                if (seenStatus == null)
                {
                    dto.Seen = false;
                }
                else
                {
                    dto.Seen = true;
                }
                results.Add(dto);
            }

            return Request.CreateResponse(results);
        }

        [Route("api/Posts/postWasSeen/{id}")]
        [HttpGet]
        [Authorize]
        public HttpResponseMessage PostWasSeen(int id)
        {
            string currentUserId = User.Identity.GetUserId();
            ApplicationUser currentUser = uow.UserRepository.GetByID(currentUserId);

            Post post = uow.PostRepository.GetByID(id);
            if (post == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "La publication n'a pas été trouvée ");
            }
            SeenPosts wasAlreadySeen = currentUser.SeenPosts.Where(x => x.Post == post).FirstOrDefault();
            if (wasAlreadySeen == null)
            {
                SeenPosts seen = new SeenPosts
                {
                    Seen = true,
                    Post = post,
                    User = currentUser
                };
                currentUser.SeenPosts.Add(seen);
                uow.Save();
            }

            return Request.CreateResponse(HttpStatusCode.OK);
        }
    }
}
