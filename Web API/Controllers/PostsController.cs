using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
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
                var pictures = new List<PictureDTO>();
                foreach (Picture picture in post.Pictures)
                {
                    PictureDTO pi = new PictureDTO();
                    pi.Base64 = picture.Base64;
                    pi.Id = picture.Id;
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
                    //Activity = new Activity("Super Activité")
                });
            }
            return postsDTO;
        }

        //Méthode de test pour obtenir un byte[] à partir d'une image dans postman
        [HttpPost]
        [Route("api/Picturetobyte")]
        public List<byte[]> GetByte()
        {
            var pictures = new List<Picture>();
            var files = HttpContext.Current.Request.Files.Collection();
            List<byte[]> picsBytes = new List<byte[]>();
            foreach (var file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    var bytes = memoryStream.ToArray();
                    var picture = new PictureWithDatabaseStorageStrategy
                    {
                        Content = bytes
                    };
                    pictures.Add(picture);
                    picsBytes.Add(picture.Content);
                }
            }
            
            return picsBytes;
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
            var pictures = new List<Picture>();
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

                foreach(CreatePictureDTO picDTO in value.CreatePicturesDTO)
                {
                    Picture pi = new Picture();
                    pi.Base64 = picDTO.Base64;
                    pictures.Add(pi);
                }
                po.Pictures = pictures;
                //TODO: Changer le user et l'activity
                //po.UserId = db.Users.FirstOrDefault(x => x.Id == value.UserId.ToString());
                //po.Activity = db.Activities.FirstOrDefault(x => x.Id == value.ActivityId);
                //po.Activity = new Activity("Super Activité");
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
