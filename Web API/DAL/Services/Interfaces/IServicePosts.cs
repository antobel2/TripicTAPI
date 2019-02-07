using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Web_API.Models;

namespace Web_API.DAL.Services
{
    public interface IServicePosts
    {
        Post GetPostById(int id);
        List<Post> GetAllPosts();
        List<Post> GetPostsForActivity(Activity activity);
        void CreatePost(Post post);
        void DeletePost(Post post);
        void DeletePostById(int id);
        void UpdatePost(Post post);
        PostDTO ToPostDTO(Post post);
    }
}
