using Microsoft.EntityFrameworkCore;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	[Inject]
	public class GameContext : DbContext, IGameContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<GameResult> Results { get; set; }

		protected override void OnConfiguring(DbContextOptionsBuilder options)
			=> options.UseSqlite("Data Source=game.db");
	}
}
