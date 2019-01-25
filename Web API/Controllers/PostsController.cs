using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
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
        [Route("api/posts")]
        public IEnumerable<Post> GetPosts()
        {
            return db.Posts;
        }

        //// GET api/values/5
        ////Retourne la liste des posts associés (ID de l'activité recherchée)
        //[Route("api/activity/{id}")]
        //public IEnumerable<Post> GetPostsByActivity(int id)
        //{
        //    return db.Posts.Where(x => x.Activity.Id == id)
        //        .ToList();
        //}

        // POST api/values
        //Crée un post
        [HttpPost]
        [Route("api/newPost")]
        public IHttpActionResult Post([FromBody]newPost value)
        {
            Post po;

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            if (!value.Text.Equals(""))
            {
                po = new Post(value.Text);
                
            }
            else
            {
                po = new Post();
            }
            
            po.Pictures = value.Pictures;
            return CreatedAtRoute("api/newPost", new { id = po.Id }, po);
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
