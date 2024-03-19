using BlogApp.Database;
using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository
{
    public class UserRepository : IUserRepository
    {
        private readonly BlogDbContext _blogDbContext;

        public UserRepository(BlogDbContext blogDbContext)
        {
            _blogDbContext = blogDbContext;
        }

        public List<Blog> GetUserBlogs(int id)
        {
            var userBlogs = _blogDbContext.Blogs.Where(b => b.UserId == id).ToList();
            return userBlogs;
        }

        public User GetUserbyEmail(string email)
        {
            return _blogDbContext.User.FirstOrDefault(u => u.Email.ToLower() == email.ToLower());
        }

        public void Upsert(User user)
        {
            var existingUser = _blogDbContext.User.FirstOrDefault(u => u.Email == user.Email);
            if (user.Id == 0) 
            {
                if (existingUser != null)
                {
                    throw new Exception("User Already register with email");
                }

                _blogDbContext.User.Add(user);
            }
            else
            {
                existingUser.Name = user.Name;
                existingUser.Email = user.Email;
                existingUser.Password = user.Password;
                existingUser.ImageUrl = user.ImageUrl;
            }

            _blogDbContext.SaveChanges();
        }
    }
}
