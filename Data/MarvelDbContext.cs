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
        public DbSet<Movie> Movies { get; set; }

    }
}
