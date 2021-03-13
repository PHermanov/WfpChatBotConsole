using Microsoft.EntityFrameworkCore;
using System.Threading;
using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.Data
{
	public interface IApplicationDbContext
	{
		public DbSet<Player> Players { get; set; }
		public DbSet<GameResult> Results { get; set; }
		public DbSet<User> Users { get; set; }
		public DbSet<Chat> Chats { get; set; }

		Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
	}
}
