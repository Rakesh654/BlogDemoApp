using BlogApp.Models;
using BlogApp.Repository;
using BlogApp.ViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Text;

namespace BlogApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private static HttpClient _httpClient;
        private static IWebHostEnvironment _webHostEnvironment;
        private readonly IUserRepository _userRepository;
        private readonly ICommentRepository _commentRepository;
        private string baseUrl;

        static HomeController()
        {
            _httpClient = new HttpClient();
        }

        public HomeController(ILogger<HomeController> logger, IHttpContextAccessor httpContextAccessor, 
            IWebHostEnvironment webHostEnvironment, IUserRepository userRepository, ICommentRepository commentRepository)
        {
            _logger = logger;
            _httpContextAccessor = httpContextAccessor;
            _webHostEnvironment = webHostEnvironment;
            _userRepository = userRepository;
            _commentRepository = commentRepository;
            var request = httpContextAccessor.HttpContext?.Request;
            baseUrl = $"{request?.Scheme}://{request?.Host}/api/blogapi";
        }

        public IActionResult Index()
        {
            var http =  _httpClient.GetAsync(baseUrl).GetAwaiter().GetResult();
            var response = http.Content.ReadFromJsonAsync<List<Blog>>().GetAwaiter().GetResult();
            return View(response);
        }

        [Authorize]
        public IActionResult Create()
        {
           return View();
        }

        [Authorize]
        public IActionResult Edit(int id)
        {
            if (id > 0)
            {
                var url = $"{baseUrl}/{id}";
                var result = _httpClient.GetAsync(url).GetAwaiter().GetResult();
                var response = result.Content.ReadFromJsonAsync<Blog>().GetAwaiter().GetResult();
                var blogDetail = new BlogCreateVM();
                blogDetail.Id = id;
                blogDetail.Title = response.Title;
                blogDetail.Description = response.Description;
                blogDetail.ImageUrl = response.ImageUrl;
                return View(blogDetail);
            }
            return View();
        }

        [HttpPost]
        public IActionResult Edit(BlogCreateVM blogVm, IFormFile? file)
        {
            if(ModelState.IsValid) 
            {
                if(file != null) 
                {
                    string wwwRootPath = _webHostEnvironment.WebRootPath;
                    if (!string.IsNullOrEmpty(blogVm.ImageUrl)) 
                    {
                        if (!blogVm.ImageUrl.Contains("http"))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, blogVm.ImageUrl.TrimStart('\\'));

                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }
                    }

                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(wwwRootPath, @"images\blog");

                    using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    var url = @"\images\blog\" + fileName;
                    blogVm.ImageUrl = url;
                }

                string data = JsonConvert.SerializeObject(blogVm);
                StringContent postContent = new StringContent(data, Encoding.UTF8, "application/json");
                var result = _httpClient.PutAsync(baseUrl, postContent).GetAwaiter().GetResult();
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("Index", "Home");
                }
            }

           return View();
        }

        [Authorize]
        [HttpPost]
        public IActionResult Create(BlogCreateVM blogObj, IFormFile? file)
        {
            //var blog = new BlogDataModel();
            if(ModelState.IsValid)
            {
               
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string path = Path.Combine(wwwRootPath, @"images\blog");

                    using (var filestream = new FileStream(Path.Combine(path, fileName), FileMode.Create))
                    {
                        file.CopyTo(filestream);
                    }

                    var url = @"\images\blog\" + fileName;
                    blogObj.ImageUrl = url;
                }

                
                string data = JsonConvert.SerializeObject(blogObj);
                StringContent postContent = new StringContent(data, Encoding.UTF8, "application/json");
                var result = _httpClient.PostAsync(baseUrl, postContent).GetAwaiter().GetResult();
                if (result.IsSuccessStatusCode)
                {
                    return RedirectToAction("ManageBlog", "User", new { email = blogObj.Email });
                }
            }
            
            return View();
        }

        [Authorize]
        public IActionResult Delete(Blog blog)
        {
            return View(blog);
        }

        [Authorize]
        [HttpPost]
        public IActionResult Delete(int id, string email)
        {
            var url = string.Format("{0}/Delete/{1}", baseUrl, id);
            var result = _httpClient.DeleteAsync(url).GetAwaiter().GetResult();
            if(result.IsSuccessStatusCode)
            {
                return RedirectToAction("ManageBlog", "User", new {email = email });
            }

            return View();

        }

        public IActionResult BlogDetail(int id) 
        {
            var blogvm = new BlogVM();
            var url = $"{baseUrl}/{id}";
            var result = _httpClient.GetAsync(url).GetAwaiter().GetResult();
            var response = result.Content.ReadFromJsonAsync<Blog>().GetAwaiter().GetResult();
            blogvm.blog = response;
            blogvm.Comments = _commentRepository.GetCommentsByBlogId(id);
            return View(blogvm);
        }
    }
}