using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Telegram.Bot;

namespace WfpBotConsole.Bootstrap
{
	public static class TelegramBotClientBootstrap
	{
		public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ITelegramBotClient>(_ => new TelegramBotClient(File.ReadAllText("key.secret")));

			return serviceCollection;
		}
	}
}
