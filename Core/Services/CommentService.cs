using Core.Services.Interfaces;
using Datalayer.Context;
using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services
{
    public class CommentService:ICommentService
    {
        private ApplicationDbContext _context;
        public CommentService(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<Comment> GetAllComments()
        {
            return _context.Comments.Include(c => c.Product).OrderByDescending(s => s.CreationDate).ToList();
        }

        public List<Comment> GetAllComments(string search)
        {
            return _context.Comments.Include(c => c.Product).Where(c=>c.Message.Contains(search) || c.Name.Contains(search)).OrderByDescending(s => s.CreationDate).ToList();
        }

        public List<Comment> GetAllComments(int ProductId)
        {
            return _context.Comments.Where(c => c.ParentId == null && c.ProductId == ProductId).OrderByDescending(c => c.Id).ToList();
        }

        public List<Comment> GetSpecificComments(int page, int countPerPage)
        {
            return _context.Comments.Include(c=>c.Product).OrderByDescending(u => u.CreationDate).Skip(countPerPage * (page - 1)).Take(countPerPage).ToList();
        }

        public Comment GetCommentById(int id)
        {
            return _context.Comments.Include(c => c.Product).FirstOrDefault(x => x.Id == id);
        }

        public void DeleteComment(int id)
        {
            var comment = GetCommentById(id);

            var repliedComment = GetRepliedComment(id);
            foreach (var rComment in repliedComment)
            {
                _context.Comments.Remove(rComment);
            }
            _context.Comments.Remove(comment);
            _context.SaveChanges();
        }

        public List<Comment> GetRepliedComment(int CommentId)
        {
            return _context.Comments.Where(c => c.ParentId == CommentId).ToList();
        }

        public void insertComment(string fullName, string email, string commentText, int ProductId)
        {
            Comment newComment = new Comment()
            {
                CreationDate = DateTime.Now,
                Email = email,
                IsRead = false,
                Message = commentText,
                Name = fullName,
                ParentId = null,
                ProductId = ProductId,

            };

            _context.Comments.Add(newComment);
            _context.SaveChanges();
        }

        public int GetUnreadCommentNumber()
        {
            return _context.Comments.Where(c=>c.IsRead == false).Count();
        }

        public void DoUnreadAllComment()
        {
            var comments = _context.Comments.Where(c => c.IsRead == false);
            foreach (var comment in comments)
            {
                comment.IsRead = true;
            }

            _context.SaveChanges();
        }

        public Comment ReplyComment(int CommentId, int ProductId, string commentText,string name)
        {
            Comment newComment = new Comment();

            newComment.ParentId = CommentId;
            newComment.ProductId = ProductId;
            newComment.CreationDate = DateTime.Now;
            newComment.Message = commentText;
            newComment.IsRead = true;
            newComment.Name = name;

            _context.Comments.Add(newComment);
            _context.SaveChanges();

            return GetCommentById(newComment.Id);
        }
    }
}
