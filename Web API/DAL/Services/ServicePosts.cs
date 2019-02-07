using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Web_API.Models;

namespace Web_API.DAL.Services
{
    public class ServicePosts : IServicePosts
    {
        private UnitOfWork uow = new UnitOfWork();

        public ServicePosts()
        {
            
        }

        public void CreatePost(Post post)
        {
                uow.PostRepository.Insert(post);
        }

        public void DeletePost(Post post)
        {
            uow.PostRepository.Delete(post);
        }

        public void DeletePostById(int id)
        {
            uow.PostRepository.Delete(id);
        }

        public List<Post> GetAllPosts()
        {
            return uow.PostRepository.Get().ToList();
        }

        public Post GetPostById(int id)
        {
            return uow.PostRepository.GetByID(id);
        }

        public List<Post> GetPostsForActivity(Activity activity)
        {
            List<Post> results = uow.PostRepository.Get(x => x.Activity.Id == activity.Id).ToList();
            return results;
        }

        public PostDTO ToPostDTO(Post post)
        {
            PostDTO result = new PostDTO
            {
                Id = post.Id,
                Text = post.Text,
                PicturesDTO = new List<PictureDTO>()
            };
            foreach (Picture picturesInPost in post.Pictures)
            {
                result.PicturesDTO.Add(new PictureDTO
                {
                    Id = picturesInPost.Id,
                    Base64 = picturesInPost.Base64
                });
            }
            return result;
        }

        public void UpdatePost(Post post)
        {
            throw new NotImplementedException();
        }
    }
}