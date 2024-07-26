using Microsoft.EntityFrameworkCore;

namespace AuthServer.Models
{
    public class CustomDbContext : DbContext
    {
        public CustomDbContext(DbContextOptions<CustomDbContext> options) : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<CustomUser>().HasData(
                new CustomUser() { Id= 1, Username = "ahmetsarikaya", Email="ahmetsarikaya0696@gmail.com", City="Ankara", Password="password"},
                new CustomUser() { Id= 2, Username = "erdalsarikaya", Email="erdalsarikaya@gmail.com", City="Istanbul", Password="password"},
                new CustomUser() { Id= 3, Username = "mehmet", Email="mehmet@gmail.com", City="Konya", Password="password"}
                );

            base.OnModelCreating(modelBuilder);
        }

        public DbSet<CustomUser> CustomUsers { get; set; }
    }
}
