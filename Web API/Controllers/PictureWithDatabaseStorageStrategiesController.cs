using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using System.Web.Http.Description;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class PictureWithDatabaseStorageStrategiesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        [HttpPost]
        [ValidateFiles]
        public HttpResponseMessage Create()
        {
            var pictures = new List<Picture>();

            var files = HttpContext.Current.Request.Files.Collection();
            foreach (var file in files)
            {
                using (var memoryStream = new MemoryStream())
                {
                    file.InputStream.CopyTo(memoryStream);
                    var bytes = memoryStream.ToArray();
                    var picture = new PictureWithDatabaseStorageStrategy
                    {
                        MimeType = file.ContentType,
                        FilenameWithExtension = file.FileName,
                        Content = bytes
                    };
                    pictures.Add(picture);
                }
            }
            db.Pictures.AddRange(pictures);
            db.SaveChanges();

            return Request.CreateResponse(HttpStatusCode.OK, pictures.Select(p => new
            {
                p.Id,
                p.MimeType,
                p.FilenameWithExtension
            }));
        }

      
        [HttpGet]
        public HttpResponseMessage GetPicture(int id)
        {
            var picture = db.Pictures.Find(id);
            if(picture == null)
            {
                return Request.CreateErrorResponse(HttpStatusCode.NotFound, "Image non trouvée");
            }
            var pictureWithDatabaseStorageStrategy = (PictureWithDatabaseStorageStrategy)picture;
            HttpResponseMessage result = new HttpResponseMessage(HttpStatusCode.OK);
            result.Content = new ByteArrayContent(pictureWithDatabaseStorageStrategy.Content);
            result.Content.Headers.ContentDisposition = new System.Net.Http.Headers.ContentDispositionHeaderValue("attachment");
            result.Content.Headers.ContentDisposition.FileName = picture.FilenameWithExtension;
            result.Content.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue(picture.MimeType);
            result.Content.Headers.ContentLength = pictureWithDatabaseStorageStrategy.Content.Length;
            return result;
        }
    }
}