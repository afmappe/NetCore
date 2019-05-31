using Microsoft.EntityFrameworkCore;
using NetCore.WebAppi.Data.Mapper;
using NetCore.WebAppi.Identity;

namespace NetCore.WebAppi.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, string schema = null)
            : base(options)
        {
            Schema = !string.IsNullOrEmpty(schema) ? schema : "dbo";
        }

        public string Schema { get; set; }

        public DbSet<UserInfo> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.HasDefaultSchema(Schema);
            modelBuilder.ApplyConfiguration(new UserMapper(Schema));
        }
    }
}