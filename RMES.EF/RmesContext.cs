using Microsoft.EntityFrameworkCore;
using RMES.Entity;

namespace RMES.EF
{
    public class RmesContext : DbContext
    {
        public RmesContext(DbContextOptions<RmesContext> options) : base(options)
        { }

        public DbSet<Topic> Topics { get; set; }

        public DbSet<Post> Posts { get; set; }

        public DbSet<History> Histories { get; set; }

        public DbSet<LikeLog> LikeLogs { get; set; }

        public DbSet<Reply> Replies { get; set; }

        public DbSet<User> Users { get; set; }

        public DbSet<Collect> Collects { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.Entity<Post>().HasQueryFilter(p => p.IsDel == false);
            builder.Entity<Topic>().HasQueryFilter(p => p.IsDel == false);
            builder.Entity<Reply>().HasQueryFilter(p => p.IsDel == false);
        }
    }
}
