using Datalayer.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Services.Interfaces
{
    public interface ICommentService
    {
        public List<Comment> GetAllComments();

        public List<Comment> GetAllComments(string search);

        public List<Comment> GetAllComments(int ProductId);

        public List<Comment> GetSpecificComments(int page, int countPerPage);

        public Comment GetCommentById(int id);

        public List<Comment> GetRepliedComment(int CommentId);

        public void insertComment(string fullName, string email, string commentText, int ProductId);

        public void DeleteComment(int id);
        public int GetUnreadCommentNumber();
        public void DoUnreadAllComment();

        public Comment ReplyComment(int CommentId, int ProductId, string commentText, string name);
    }
}
