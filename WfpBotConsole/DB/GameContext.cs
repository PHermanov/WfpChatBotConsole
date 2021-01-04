using Microsoft.EntityFrameworkCore;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
    public class GameContext : DbContext
    {
        public DbSet<Player> Players { get; set; }
        public DbSet<GameResult> Results { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder options)
            => options.UseSqlite("Data Source=game.db");
    }
}
