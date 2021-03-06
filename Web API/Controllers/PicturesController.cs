﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;
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

        ////Permet de retourner toutes les photos d'un user
        //[HttpGet]
        //[Route("api/Picture")]
        //public HttpResponseMessage GetPicture()
        //{
        //    var data = uow.PictureRepository.Get()
        //        .Select(p => new
        //        {
        //            p.Id,
        //            p.Base64
        //        });
        //    return Request.CreateResponse(data);
        //}

        //Permet de retourner la photo demandée par son Id
        [HttpGet]
        [Route("api/Pictures/GetPictureFromId/{id}")]
        public HttpResponseMessage GetPictureFromId(int id)
        {

            var data = uow.PictureRepository.GetByID(id);
            byte[] bytes = Convert.FromBase64String(GetBase64String(data.Base64));
            HttpResponseMessage result;
            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);

                result = new HttpResponseMessage(HttpStatusCode.OK)
                {
                    Content = new ByteArrayContent(ms.ToArray())
                };
                result.Content.Headers.ContentType = new MediaTypeHeaderValue("image/png");
            }
            return result;
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

        public static string GetBase64String(string s)
        {
            int base64StringStart = s.IndexOf(',');
            string finalS = s.Substring(base64StringStart + 1).Trim();
                return finalS;
        }

        //Obtient le type de fichier du fichier et vérifie qu'il s'agit des bons types de fichiers
        private static String ExtractMimeType(String s)
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
        private static String ExtractExtension(String s)
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
        [Authorize]
        [Route("api/Pictures/CreatePicture")]
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
            else if (ExtractExtension(value.Base64) == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "L'image doit être au format .png, .jpeg ou .gif");
            else if (ExtractMimeType(value.Base64) == null)
                return Request.CreateErrorResponse(HttpStatusCode.BadRequest, "Le média doit être une image");
            
            //Si tout passe, crée la photo à partir du body de la réponse, l'ajoute à la bd et renvoie un code 200 au client
            else
            {
                Picture p = new Picture
                {
                    Base64 = value.Base64,
                    Post = uow.PostRepository.GetByID(value.PostId)
                };
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
        [Authorize]
        [Route("api/Pictures/{id}")]
        public HttpResponseMessage GetPicturesByPost(int id)
        {
            var data = uow.PictureRepository.Get(a => a.Post.Id == id)
                .ToList();
            return Request.CreateResponse(data);

        }
    }
}
