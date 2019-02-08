using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;
using Web_API.DAL;
using Web_API.Models;

namespace Web_API.Controllers
{
    public class PicturesController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();
        private UnitOfWork uow = new UnitOfWork();

        //Permet de retourner toutes les photos d'un user
        [HttpGet]
        [Route("api/Picture")]
        public HttpResponseMessage GetPicture()
        {
            var data = uow.PictureRepository.Get()
                .Select(p => new
                {
                    p.Id,
                    p.Base64
                });
            return Request.CreateResponse(data);
        }

        //Permet de retourner toutes les photos d'un user
        [HttpGet]
        [Route("api/Picture/GetPictureFromId/{id}")]
        public HttpResponseMessage GetPictureFromId(int id)
        {
            var data = uow.PictureRepository.Get()
                .Select(p => new
                {
                    p.Id,
                    p.Base64
                });
            return Request.CreateResponse(data);
        }

        //Méthode pour vérifier que la string reçue est bien une image en base 64
        //Doit être un multiple de 4, contenir seulement les caractères spécifiés et peut terminer par '='
        public static bool IsBase64String(string s)
        {
            int base64StringStart = s.IndexOf(',');
            string finalS = s.Substring(base64StringStart + 1).Trim();
            if ((finalS.Length % 4 == 0) && Regex.IsMatch(finalS, @"^[a-zA-Z0-9\+/]*={0,3}$", RegexOptions.None))
                return true;
            else
                return false;
        }

        //Obtient le type de fichier du fichier et vérifie qu'il s'agit des bons types de fichiers
        private static String extractMimeType(String s)
        {
            //Détermine où se trouve la string requise
            int extentionStartIndex = s.IndexOf('/');
            int filetypeStartIndex = s.IndexOf(':');

            String fileType = s.Substring(filetypeStartIndex + 1, extentionStartIndex - (filetypeStartIndex + 1));

            if (fileType != "image")
            {
                return null;
            }

            return fileType;
        }

        //Obtient l'extension du fichier et vérifie qu'elle est valide
        private static String extractExtension(String s)
        {
            //Détermine où se trouve la string requise
            int extentionStartIndex = s.IndexOf('/');
            int extensionEndIndex = s.IndexOf(';');

            String fileType = s.Substring(extentionStartIndex + 1, extensionEndIndex - (extentionStartIndex + 1));

            if (fileType != "jpeg" && fileType != "gif" && fileType != "png")
            {
                return null;
            }

            return fileType;
        }

        //Crée la photo après avoir vérifie qu'elle passe toutes les étapes de validation
        [HttpPost]
        [Route("api/Picture/CreatePicture")]
        public HttpResponseMessage CreatePicture([FromBody]CreatePictureDTO value)
        {
            //Valide les informations fournies
            //Renvoie une réponse Http avec un message d'erreur si la vérification ne passe pas

            if (value == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Il n'y a pas d'image");
            else if (value.Base64 == null || value.Base64.Trim() == "")
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Image vide");
            else if (!IsBase64String(value.Base64))
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'information doit contenir une string de Base64");
            else if (extractExtension(value.Base64) == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'image doit être au format .png, .jpeg ou .gif");
            else if (extractMimeType(value.Base64) == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le média doit être une image");
            
            //Si tout passe, crée la photo à partir du body de la réponse, l'ajoute à la bd et renvoie un code 200 au client
            else
            {
                Picture p = new Picture();
                p.Base64 = value.Base64;
                p.Post = uow.PostRepository.GetByID(value.PostId);
                uow.PictureRepository.Insert(p);

                
                if (p.Post.PicNumber == p.Post.Pictures.Count)
                {
                    p.Post.IsValid = true;
                    uow.PostRepository.Update(p.Post);
                }
                    
                return Request.CreateResponse(HttpStatusCode.OK);
            }
        }

        //Permet de retourner les photos associées à un post
        [HttpGet]
        [Route("api/Picture/{id}")]
        public HttpResponseMessage GetPicturesByPost(int id)
        {
            var data = uow.PictureRepository.Get(a => a.Post.Id == id)
                .ToList();
            return Request.CreateResponse(data);

        }
    }
}
