using BlogApp.Repository;
using BlogApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Controllers
{
    public class CommentController : Controller
    {
        private readonly ICommentRepository _commentRepository;
        private readonly IUserRepository _userRepository;

        public CommentController(ICommentRepository commentRepository, IUserRepository userRepository)
        {
            _commentRepository = commentRepository;
            _userRepository = userRepository;
        }
        public IActionResult Index(int blogId, int? commentId)
        {
            var model = new CommentVM();
            if (commentId != null && commentId != 0)
            {
                var commment = _commentRepository.GetCommentbyId(commentId.Value);
                model.Comment = commment.Comment;
                model.Id = commment.Id;
                model.CreatedBy = commment.CreatedBy;
            }
            
            ViewBag.blogId = blogId;
            return View(model);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(CommentVM commentVM)
        {
            var userDetail = _userRepository.GetUserbyEmail(commentVM.CreatedBy);
            commentVM.UserId = userDetail.Id;
            if (userDetail != null)
            {
                _commentRepository.Upsert(commentVM);
                return RedirectToAction("BlogDetail", "Home", new { id = commentVM.BlogId });
            }

            return View(commentVM);
        }
    }
}
