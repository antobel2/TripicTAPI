using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class PicturesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        //Permet de retourner toutes les photos d'un user
        [HttpGet]
        [Route("api/Picture")]
        public HttpResponseMessage GetPicture()
        {

            var data = db.Pictures.ToList()
                .Select(p => new
                {
                    p.Id,
                    p.MimeType,
                    p.FilenameWithExtension
                });
            return Request.CreateResponse(data);
            
        }

        //Permet de retourner les photos associées à un post
        [HttpGet]
        [Route("api/Picture/{id}")]
        public HttpResponseMessage GetPicturesByPost(int id)
        {
            var data = db.Pictures.Where(a => a.Post.Id == id)
                .ToList();
            return Request.CreateResponse(data);

        }
    }
}
