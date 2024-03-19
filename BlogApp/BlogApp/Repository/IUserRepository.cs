using BlogApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace BlogApp.Repository
{
    public interface IUserRepository
    {
        void Upsert(User user);
        List<Blog> GetUserBlogs(int id);

        User GetUserbyEmail(string email);

    }
}
