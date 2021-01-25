using Microsoft.Extensions.DependencyInjection;
using WfpBotConsole.Commands;
using WfpBotConsole.Services;

namespace WfpBotConsole.Bootstrap
{
	public static class CommandsBootstrap
	{
		public static IServiceCollection AddCommands(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<ICommand, AllWinnersCommand>();
			serviceCollection.AddTransient<ICommand, CheckMissedGamesCommand>();
			serviceCollection.AddTransient<ICommand, CurrentMonthTopWinnersCommand>();
			serviceCollection.AddTransient<ICommand, HelpCommand>();
			serviceCollection.AddTransient<ICommand, NewWinnerCommand>();
			serviceCollection.AddTransient<ICommand, ShowUsersCommand>();
			serviceCollection.AddTransient<ICommand, TestCommand>();
			serviceCollection.AddTransient<ICommand, WinnerTodayRuCommand>();
			serviceCollection.AddTransient<ICommand, WinnerTodayUkrCommand>();
			serviceCollection.AddTransient<ICommand, WinnerYesterdayCommand>();

			serviceCollection.AddSingleton<ICommandsService, CommandsService>();

			return serviceCollection;
		}
	}
}
