using Microsoft.Extensions.DependencyInjection;
using WfpBotConsole.Services;

namespace WfpBotConsole.Bootstrap
{
	public static class ServicesBootstrap
	{
		public static IServiceCollection AddServices(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<IAutoReplyService, AutoReplyService>();

			return serviceCollection;
		}
	}
}
