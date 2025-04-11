using Microsoft.EntityFrameworkCore;
using word_game.Models;

namespace word_game.ConnectContext
{
    public class ColodContext : DbContext
    {
        public DbSet<ModelColod> Colods { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=GameWpf;Username=postgres;Password=1234");
        }
    }
}
