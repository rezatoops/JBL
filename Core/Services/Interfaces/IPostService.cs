using Datalayer.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface IPostService
    {
        public List<BlogCategory> GetAllCategories();

        public string GetUnicSlug(string slug, int id);

        public void SavePost(Post post);

        public void AddCategoryToPost(int postId, string[] categories);

        public void AddTagsToPost(string TagNames, int postId);

        public List<Post> GetAllPosts();

        public List<Post> GetSpecificPosts(int page, int countPerPage);

        public List<PostTag> GetPostTags(int postId);

        public List<string> GetPostCategories(int posdtId);

        public Post GetPost(int id);

        public void SaveDatabse();

        public void UpdatePostCategories(int postId, string[] categories);

        public void UpdatePostTag(string TagNames, int postId);

        public void DeletePost(int id);

        public List<Post> GetPostByCategoryId(int CategoryId);

        public string GetTextSummary(string fullText, int numberChar);

        public Post GetPostBySlug(string slug);

        public BlogCategory GetCategoryById(int catId);

        public void SaveBlogCategory(BlogCategory blogCategory);

        public void DeleteCategory(int Id);

    }
}
