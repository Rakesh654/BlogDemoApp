using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using Microsoft.Build.Framework;

namespace BlogApp.ViewModel
{
    public class BlogCreateVM
    {
        public int? Id { get; set; }
        [Required]
        public string Title { get; set; }
        [Required]
        public string Description { get; set; }

        [Required]
        public string Email { get; set; }

        [ValidateNever]
        public string ImageUrl { get; set; }
    }
}
