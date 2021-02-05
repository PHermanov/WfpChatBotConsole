using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using WfpBotConsole.DB;

namespace WfpBotConsole.Bootstrap
{
	public static class DBBootstrap
	{
		public static IServiceCollection AddDataBase(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddDbContext<GameContext>(s => s.UseSqlite("Data Source=game.db"));

			return serviceCollection;
		}
	}
}
