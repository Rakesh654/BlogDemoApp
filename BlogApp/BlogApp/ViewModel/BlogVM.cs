using BlogApp.Models;

namespace BlogApp.ViewModel
{
    public class BlogVM
    {
        public Blog? blog { get; set; }
        public List<CommentModel>? Comments { get; set; }
    }
}
