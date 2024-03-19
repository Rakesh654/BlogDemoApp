using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace BlogApp.Models
{
    public class BlogDataModel
    {
        [ValidateNever]
        public int Id { get; set; }

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [AllowNull]
        [ValidateNever]
        public string ImageUrl { get; set; }

        [AllowNull]
        public string CreatedBy { get; set; }

        [AllowNull]
        public string Author { get; set; }

        [AllowNull]
        public DateTime CreatedTime { get; set; }
    }
}
