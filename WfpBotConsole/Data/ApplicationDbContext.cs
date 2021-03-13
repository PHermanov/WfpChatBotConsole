using Microsoft.EntityFrameworkCore;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Models;

namespace WfpBotConsole.Data
{
	[Inject]
	public class ApplicationDbContext : DbContext, IApplicationDbContext
	{
		public DbSet<Player> Players { get; set; }

		public DbSet<GameResult> Results { get; set; }

		public DbSet<User> Users { get; set; }

		public DbSet<Chat> Chats { get; set; }

		public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
			: base(options)
		{
		}
	}
}
