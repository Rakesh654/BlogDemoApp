using Microsoft.Build.Framework;

namespace BlogApp.ViewModel
{
    public class CommentVM
    {
        public int? Id { get; set; }

        [Required]
        public string Comment { get; set; }
        [Required]
        public string CreatedBy { get; set; }

        public DateTime? CreatedTime { get; set; }
        public int? UserId { get; set; }
        public int? BlogId { get; set; }
    }
}
