using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	public class GameContext : DbContext, IGameContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<GameResult> Results { get; set; }

		public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
		{
			return base.SaveChangesAsync(cancellationToken);
		}

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=game.db");
	}
}
