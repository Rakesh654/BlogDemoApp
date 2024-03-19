using Microsoft.EntityFrameworkCore;

namespace Blog.DataAccess.Database
{
    public class BlogDbContext : DbContext
    {
        public BlogDbContext(DbContextOptions<BlogDbContext> options) : base(options)
        {
            
        }
    }
}
