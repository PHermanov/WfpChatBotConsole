using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	public interface IGameContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<GameResult> Results { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
