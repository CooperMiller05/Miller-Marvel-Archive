using MarvelArchive.Models;
using Microsoft.EntityFrameworkCore;

namespace MarvelArchive.Data
{
    public class MarvelDbContext : DbContext
    {
        public MarvelDbContext(DbContextOptions<MarvelDbContext> options) : base(options)
        {
        }

        public DbSet<Character> Characters { get; set; }

        public DbSet<Media> Media { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<CharacterMedia>().HasKey(cm => new { cm.CharacterId, cm.MediaId });
        }

        public DbSet<CharacterMedia> CharacterMedia { get; set; }
    }
}
