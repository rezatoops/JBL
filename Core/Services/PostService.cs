using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using HtmlAgilityPack;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class PostService : IPostService
    {
        private ApplicationDbContext _context;
        public PostService(ApplicationDbContext context)
        {
            _context = context;
        }
        public List<BlogCategory> GetAllCategories()
        {
            return _context.BlogCategories.ToList();
        }

        public string GetUnicSlug(string slug, int id)
        {
            bool isSlugExist = false;
            while (!isSlugExist)
            {
                var q = from a in _context.Posts where a.Slug == slug select a;
                if (q.Count() > 0)
                {
                    if (q.Count() == 1 && q.FirstOrDefault().Id == id)
                    {
                        isSlugExist = true;
                    }
                    else
                    {
                        slug = slug + "_1";
                        isSlugExist = false;
                    }
                }
                else
                {
                    isSlugExist = true;
                }
            }

            return slug;

        }

        public void SavePost(Post post)
        {
            _context.Posts.Add(post);
            _context.SaveChanges();
        }

        public void SaveBlogCategory(BlogCategory blogCategory)
        {
            _context.BlogCategories.Add(blogCategory);
            _context.SaveChanges();
        }

        public void AddCategoryToPost(int postId, string[] categories)
        {
            foreach (var category in categories)
            {
                PostCategory postCategory = new PostCategory()
                {
                    PostId = postId,
                    BlogCategoryId = int.Parse(category),
                };

                _context.PostCategories.Add(postCategory);
            }

            _context.SaveChanges();

        }

        public void AddTagsToPost(string TagNames, int postId)
        {
            foreach (var Tag in TagNames.Split("-"))
            {
                var existTag = _context.BlogTags.Where(t => t.Name == Tag.Trim()).FirstOrDefault();

                if (existTag == null)
                {
                    BlogTag newTag = new BlogTag()
                    {
                        Name = Tag.Trim(),
                    };

                    _context.BlogTags.Add(newTag);
                    _context.SaveChanges();

                    existTag = newTag;
                }

                PostTag pt = new PostTag()
                {
                    PostId = postId,
                    BlogTagId = existTag.Id,
                };

                _context.PostTags.Add(pt);
            }

            _context.SaveChanges();
        }

        public List<Post> GetAllPosts()
        {
            return _context.Posts.Include(p => p.Media).Include(p => p.User).OrderByDescending(p => p.Id).ToList();
        }

        public List<Post> GetSpecificPosts(int page, int countPerPage)
        {
            return _context.Posts.Include(p => p.Media).Include(u => u.User).OrderByDescending(p => p.Id).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();

        }

        public List<PostTag> GetPostTags(int postId)
        {
            return _context.PostTags.Include(t => t.BlogTag).Where(pt => pt.PostId == postId).ToList();
        }

        public List<string> GetPostCategories(int posdtId)
        {
            return _context.PostCategories.Where(pc => pc.PostId == posdtId).Select(x => x.BlogCategoryId.ToString()).ToList();
        }

        public Post GetPost(int id)
        {
            return _context.Posts.Include(p => p.Media).Where(p => p.Id == id).FirstOrDefault();
        }

        public void SaveDatabse()
        {
            _context.SaveChanges();
        }

        public void DeletePostCategory(int postId)
        {
            var postCategory = _context.PostCategories.Where(pt => pt.PostId == postId).ToList();

            foreach (var category in postCategory)
            {
                _context.PostCategories.Remove(category);
            }

            _context.SaveChanges();
        }

        public void UpdatePostCategories(int postId, string[] categories)
        {
            DeletePostCategory(postId);
            AddCategoryToPost(postId, categories);
        }

        public void DeletePostTag(int postId)
        {
            var tags = _context.PostTags.Where(p => p.PostId == postId).ToList();

            foreach (var tag in tags)
            {
                _context.PostTags.Remove(tag);
            }

            _context.SaveChanges();
        }


        public void UpdatePostTag(string TagNames, int postId)
        {
            DeletePostTag(postId);

            AddTagsToPost(TagNames, postId);
        }
        public void DeletePost(int id)
        {
            var post = _context.Posts.FirstOrDefault(p => p.Id == id);
            _context.Posts.Remove(post);

            _context.SaveChanges();
        }

        public List<Post> GetPostByCategoryId(int CategoryId)
        {
            return _context.PostCategories.Include(p => p.Post).ThenInclude(p => p.Media).Include(p => p.Post).ThenInclude(p => p.User).Where(p => p.BlogCategoryId == CategoryId).Select(x => x.Post).ToList();
        }

        public string GetTextSummary(string fullText, int numberChar)
        {
            HtmlDocument mainDoc = new HtmlDocument();
            mainDoc.LoadHtml(fullText);
            string cleanText = mainDoc.DocumentNode.InnerText;
            string final = string.Join(" ", cleanText.Split(' ').Take(numberChar));
            final = final + " ...";
            return final;

        }

        public Post GetPostBySlug(string slug)
        {
            return _context.Posts.Include(p => p.Media).Include(p => p.User).Where(p => p.Slug == slug).FirstOrDefault();
        }

        public BlogCategory GetCategoryById(int catId)
        {
            return _context.BlogCategories.Where(c => c.Id == catId).FirstOrDefault();
        }

        public void DeleteCategory(int Id)
        {
            var category = GetCategoryById(Id);

            _context.BlogCategories.Remove(category);
            _context.SaveChanges();
        }
    }
}
