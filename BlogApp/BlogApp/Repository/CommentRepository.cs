using BlogApp.Database;
using BlogApp.Models;
using BlogApp.ViewModel;
using Microsoft.EntityFrameworkCore;

namespace BlogApp.Repository
{
    public class CommentRepository : ICommentRepository
    {
        private readonly BlogDbContext _db;

        public CommentRepository(BlogDbContext db)
        {
            _db = db;
        }

        public CommentModel? GetCommentbyId(int id)
        {
            try
            {
                return _db.Comments.Find(id);
            }
            catch
            {
                throw new Exception("Not able to fetch the comment");
            }
        }

        public List<CommentModel> GetCommentsByBlogId(int blogId)
        {
            try
            {
                return _db.Comments.Include("User").Where(b => b.BlogId == blogId).ToList();
            }
            catch
            {
                throw new Exception("Not able to fetch the comments");
            }
        }

        public void Remove(int commentId)
        {
            var comment = _db.Comments.Find(commentId);
            if (comment != null)
            {
                _db.Comments.Remove(comment);
            }
        }

        public void Upsert(CommentVM comment)
        {
            if(comment.Id == null)
            {
                var dbcomment = new CommentModel();
                dbcomment.Comment = comment.Comment;
                dbcomment.CommentAddedUserId = comment.UserId;
                dbcomment.BlogId = comment.BlogId;
                dbcomment.CreatedBy = comment.CreatedBy;
                dbcomment.CreatedTime = DateTime.Now;
                _db.Comments.Add(dbcomment);
                _db.SaveChanges();
            }
            else
            {
                var existingCommnent = _db.Comments.FirstOrDefault(c => c.Id == comment.Id);
                if (existingCommnent != null)
                {
                    existingCommnent.Comment = comment.Comment;
                    existingCommnent.CreatedBy = comment.CreatedBy;
                    existingCommnent.CreatedTime = DateTime.Now;
                    _db.Comments.Update(existingCommnent);
                    _db.SaveChanges(true);
                }
            }
        }
    }
}
