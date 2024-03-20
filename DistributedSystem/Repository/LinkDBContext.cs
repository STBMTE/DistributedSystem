using DistributedSystem.Model;
using Microsoft.EntityFrameworkCore;

namespace DistributedSystem.Repository
{
    public class LinkDBContext : DbContext
    {
        private static bool _firstStart = true;
        public DbSet<Link> Links {  get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=postgres;Database=link;Username=stbmte;Password=123456");
        }

        public LinkDBContext() 
        {
            if (_firstStart)
            {
                Database.EnsureDeleted();
                Database.EnsureCreated();
                _firstStart = false;
            }
        }
    }
}
