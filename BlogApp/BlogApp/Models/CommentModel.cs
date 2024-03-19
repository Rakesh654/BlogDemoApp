using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;
using System.Diagnostics.CodeAnalysis;

namespace BlogApp.Models
{
    public class CommentModel
    {
        public int Id { get; set; }
        [Required]
        public string Comment { get; set; }
        public string CreatedBy { get; set; }

        public DateTime CreatedTime { get; set; }
        public int? CommentAddedUserId { get; set; }

        [ForeignKey("CommentAddedUserId")]
        public User? User { get; set; }
        public int? BlogId { get; set; }

        [ForeignKey("BlogId")]
        public Blog? Blog { get; set; }
    }
}
