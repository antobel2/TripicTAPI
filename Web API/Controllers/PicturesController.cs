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

        [HttpGet]
        public HttpResponseMessage GetPictures()
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
    }
}
