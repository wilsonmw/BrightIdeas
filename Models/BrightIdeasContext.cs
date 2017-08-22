using Microsoft.EntityFrameworkCore;
 
namespace BrightIdeas.Models
{
    public class BrightIdeasContext : DbContext
    {
        // base() calls the parent class' constructor passing the "options" parameter along
        public BrightIdeasContext(DbContextOptions<BrightIdeasContext> options) : base(options) { }

        public DbSet<User> Users {get; set;}
        public DbSet<Like> Likes {get; set;}
        public DbSet<Idea> Ideas {get; set;}
    }
}
