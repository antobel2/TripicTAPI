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

        //static List<Post> posts = new List<Post> { new Post("First post"), new Post("Second post") };
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
                    //Id = post.Id,
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

        //Méthode pour vérifier que la string reçue est bien une image en base 64
        public static bool IsBase64String(string s)
        {
            int base64StringStart = s.IndexOf(',');
            string finalS = s.Substring(base64StringStart + 1).Trim();
            if ((finalS.Length % 4 == 0) && Regex.IsMatch(finalS, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
                return true;
            else
                return false;
        }

        //Obtient le type de fichier et l'extension du fichier et vérifie qu'il s'agit des bons types de fichiers
        private static String extractMimeType(String s)
        {
            int extentionStartIndex = s.IndexOf('/');
            int filetypeStartIndex = s.IndexOf(':');

            String fileType = s.Substring(filetypeStartIndex + 1, extentionStartIndex - (filetypeStartIndex + 1));

            if (fileType != "image")
            {
                return null;
            }

            return fileType;
        }
        private static String extractExtension(String s)
        {
            int extentionStartIndex = s.IndexOf('/');
            int extensionEndIndex = s.IndexOf(';');

            String fileType = s.Substring(extentionStartIndex + 1, extensionEndIndex - (extentionStartIndex + 1));

            if (fileType != "jpeg" && fileType != "gif" && fileType != "png")
            {
                return null;
            }

            return fileType;
        }

        // POST api/values
        //Crée un post
        //[HttpPost]
        //[Route("api/Post/CreatePost")]
        //public async Task<HttpResponseMessage> CreatePost([FromBody]CreatePostDTO value)
        //{
        //    if (!ModelState.IsValid)
        //    {
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, ModelState);
        //    }

        //    Post po = new Post();

        //    if (value.Text != null && value.Text.Trim() != "")
        //    {
        //        po.Text = value.Text;
        //    }

        //    if (value.PictureNumber != 0)
        //    {
        //        po.Pictures = new List<Picture>();
        //        po.IsValid = false;
                
        //        foreach(CreatePictureDTO pic in value.PicturesDTO)
        //        {
        //            HttpResponseMessage x =  await CreatePicture(pic);
        //            value.PicturesDTO.Add(x);
        //        }
        //    }
            
        //    //TODO: Changer le user et l'activity
        //    //po.UserId = db.Users.FirstOrDefault(x => x.Id == value.UserId.ToString());
        //    //po.Activity = db.Activities.FirstOrDefault(x => x.Id == value.ActivityId);
        //    //po.Activity = new Activity("Super Activité");
        //    db.Posts.Add(po);
        //    db.SaveChanges();
        //    return Request.CreateResponse(po.Id);
        //}

        //public async Task<HttpResponseMessage> CreatePicture([FromBody]CreatePictureDTO value)
        //{
        //    Picture pi = new Picture();
        //    string errorMessage;

        //    var message = new HttpResponseMessage(HttpStatusCode.BadRequest)
        //    {
        //        if (value.Base64 == null || value.Base64.Trim() == "")
        //            errorMessage = "We cannot use IDs greater than 100.";
        //        else if (!IsBase64String(value.Base64))
        //        Content = new StringContent("We cannot use IDs greater than 100.")
        //    };
        //    throw new HttpResponseException(message);

        //    if (value.Base64 == null || value.Base64.Trim() == "")
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Image vide");
        //    else if (!IsBase64String(value.Base64))
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'information doit contenir une string de Base64");
        //    else if (extractExtension(value.Base64) == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'image doit être au format .png, .jpeg ou .gif");
        //    else if (extractMimeType(value.Base64) == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le média doit être une image");

        //    pi.Base64 = value.Base64;
        //    pi.Post = db.Posts.Find(value.PostId);
        //    db.Pictures.Add(pi);

        //    using (var client = new HttpClient())
        //    {
        //        return await client.PostAsJsonAsync("Post", pi).;
        //    }
        //}

        //public HttpResponseMessage CheckPicture([FromBody]CreatePictureDTO value)
        //{
        //    Picture pi = new Picture();

        //    if (value.Base64 == null || value.Base64.Trim() == "")
        //         err Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Image vide");
        //    else if (!IsBase64String(value.Base64))
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'information doit contenir une string de Base64");
        //    else if (extractExtension(value.Base64) == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'image doit être au format .png, .jpeg ou .gif");
        //    else if (extractMimeType(value.Base64) == null)
        //        return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le média doit être une image");

        //    using (var client = new HttpClient())
        //    {
        //        return Request.CreateResponse(HttpStatusCode.OK);
        //    }
        //}



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
                    //Id = postInActivity.Id,
                    Text = postInActivity.Text,
                    PicturesDTO = new List<PictureDTO>()
                };
                //ajouter toutes les photos du post à la liste de photo du dto
                foreach (Picture picturesInPost in postInActivity.Pictures)
                {
                    currentPost.PicturesDTO.Add(new PictureDTO
                    {
                        //Id = picturesInPost.Id,
                        Base64 = picturesInPost.Base64
                    });
                }
                results.Add(currentPost);
            }

            return Request.CreateResponse(results);
        }
    }
}
