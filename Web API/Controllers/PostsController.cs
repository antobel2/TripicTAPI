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
        [Route("api/Posts")]
        public IEnumerable<Post> GetPosts()
        {
            return db.Posts;
        }

        //// GET api/values/5
        //[Route("api/values/{id}")]
        //public IEnumerable<Post> GetValues(int id)
        //{
        //}

        // POST api/values
        //Crée un post
        [HttpPost]
        [Route("api/NewPost")]
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
            return CreatedAtRoute("api/NewPost", new { id = po.Id }, po);
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
