using BlogApp.Models;
using BlogApp.ViewModel;

namespace BlogApp.Repository
{
    public interface ICommentRepository
    {
        void Upsert(CommentVM comment);
        void Remove(int commentId);

        List<CommentModel> GetCommentsByBlogId(int blogId);

        CommentModel GetCommentbyId(int id);
    }
}
