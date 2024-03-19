using BlogApp.Database;
using BlogApp.Models;
using BlogApp.ViewModel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace BlogApp.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogApiController : ControllerBase
    {
        private readonly BlogDbContext _db;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public BlogApiController(BlogDbContext db, IWebHostEnvironment webHostEnvironment)
        {
            _db = db;
            _webHostEnvironment = webHostEnvironment;
        }

        // GET: api/<BlogApiController>
        [HttpGet]
        public IEnumerable<Blog> Get()
        {
            var blogs = _db.Blogs.Include("User").ToList();
            return blogs;
        }

        // GET api/<BlogApiController>/5
        [HttpGet("{id}")]
        public Blog Get(int id)
        {
            var blog = _db.Blogs.Include("User").FirstOrDefault(u => u.Id == id);
            if (blog == null) return null;
            return blog;
        }

        // POST api/<BlogApiController>
        [HttpPost]
        public IActionResult Post([FromBody] BlogCreateVM blogvm)
        {
            try
            {
                var blog = new Blog();
                var user = _db.User.FirstOrDefault(u => u.Email == blogvm.Email);
                blog.Title = blogvm.Title;
                blog.Description = blogvm.Description;
                blog.Author = user.Name;
                blog.UserId = user.Id;
                blog.CreatedTime = DateTime.Now;
                blog.CreatedBy = user.Email;
                blog.ImageUrl = blogvm.ImageUrl;
                if (blog != null)
                {
                    _db.Blogs.Add(blog);
                    _db.SaveChanges();
                    return Ok();
                }
                return BadRequest("Not Found");
            }
            catch
            {
                throw;
            }
        }

        // PUT api/<BlogApiController>/5
        [HttpPut]
        public void Put([FromBody] BlogCreateVM blog)
        {
            try
            {
                var existblog = _db.Blogs.FirstOrDefault(u => u.Id == blog.Id);
                if (existblog == null) return;
                existblog.Title = blog.Title;
                existblog.Description = blog.Description;
                existblog.ImageUrl = blog.ImageUrl;
                _db.Blogs.Update(existblog);
                _db.SaveChanges();
            }catch 
            {
                throw new Exception("Unable to update the blog");
            }
            
        }

        // DELETE api/<BlogApiController>/5
        [HttpDelete("Delete/{id}")]
        public void Delete(int id)
        {
            try
            {
                var blog = _db.Blogs.Find(id);
                if (blog == null) return;
                if (!string.IsNullOrEmpty(blog.ImageUrl))
                {
                    if (!blog.ImageUrl.Contains("http"))
                    {
                        string wwwRootPath = _webHostEnvironment.WebRootPath;
                        var oldImagePath = Path.Combine(wwwRootPath, blog.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }
                }

                _db.Blogs.Remove(blog);
                _db.SaveChanges();
            }
            catch
            {
                throw new Exception("Unable to delete the blog");
            }
            
        }
    }
}
