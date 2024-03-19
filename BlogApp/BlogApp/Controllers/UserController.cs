using BlogApp.Models;
using BlogApp.Repository;
using BlogApp.ViewModel;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace BlogApp.Controllers
{
    public class UserController : Controller
    {
        private readonly IUserRepository _userRepository;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public UserController(IUserRepository userRepository, IWebHostEnvironment webHostEnvironment)
        {
            _userRepository = userRepository;
            _webHostEnvironment = webHostEnvironment;
        }
        // GET: UserController
        public ActionResult Index()
        {
            return View();
        }


        // GET: UserController/Create
        [HttpPost]
        public ActionResult Create(User user, IFormFile? file)
        {
            if (ModelState.IsValid && user != null)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(wwwRootPath, @"images\userProfile");

                    using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    var url = @"\images\userProfile\" + fileName;
                    user.ImageUrl = url;
                }

                user.CreatedTime = DateTime.Now;
                _userRepository.Upsert(user);
                 return RedirectToAction("Login");
            }

            return View();
        }

        // GET: UserController/Delete/5
        public ActionResult Login()
        {
            return View();
        }

        // POST: UserController/Delete/5
        [HttpPost]
        public ActionResult Login(LoginUserVM userVm)
        {
            if (ModelState.IsValid)
            {
                var user = _userRepository.GetUserbyEmail(userVm.Email);
                if (user != null)
                {
                    var isValid = (user.Email == userVm.Email && user.Password == userVm.Password);
                    if (isValid)
                    {
                        var identity = new ClaimsIdentity(new[] { new Claim(ClaimTypes.Name, user.Email) }, CookieAuthenticationDefaults.AuthenticationScheme);
                        var princilple = new ClaimsPrincipal(identity);
                        HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, princilple);
                        HttpContext.Session.SetString("Username", user.Email);
                        return RedirectToAction("Index", "Home");
                    }
                    else
                    {
                        TempData["password"] = "Invalid password";
                        return View(userVm);
                    }
                }
                else
                {
                    TempData["userError"] = "Invalid user";
                    return View(userVm);
                }
            }

            return View(userVm);
        }

        public IActionResult LogOut() 
        {
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Login");
        
        }

        [Authorize]
        public IActionResult ManageBlog(string email) 
        {
            var user = _userRepository.GetUserbyEmail(email);
            var blogs = _userRepository.GetUserBlogs(user.Id);

            return View(blogs);
        }
    }
}
