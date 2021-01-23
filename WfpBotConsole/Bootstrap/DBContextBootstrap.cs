using Microsoft.Extensions.DependencyInjection;
using WfpBotConsole.DB;

namespace WfpBotConsole.Bootstrap
{
	public static class DBContextBootstrap
	{
		public static IServiceCollection AddDBContext(this IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddTransient<IGameContext, GameContext>()
				.AddTransient<IGameRepository, GameRepository>();

			return serviceCollection;
		}
	}
}
