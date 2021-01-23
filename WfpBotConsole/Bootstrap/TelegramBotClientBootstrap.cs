using Microsoft.Extensions.DependencyInjection;
using System.IO;
using Telegram.Bot;
using WfpBotConsole.Services;

namespace WfpBotConsole.Bootstrap
{
	public static class TelegramBotClientBootstrap
	{
		public static IServiceCollection AddTelegramBotClient(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddSingleton<ITelegramBotClient>(new TelegramBotClient(File.ReadAllText("key.secret")));
			serviceCollection.AddSingleton<ITelegramBotService, TelegramBotService>();

			return serviceCollection;
		}
	}
}
