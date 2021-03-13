using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WfpBotConsole.Bootstrap;
using WfpBotConsole.Data;
using WfpBotConsole.Jobs;
using WfpBotConsole.Services;

namespace WfpBotConsole
{
	public static class ApplicationHost
	{
		private static ITelegramBotService _telegramBotService;
		private static IJobManagerHelper _jobManagerHelper;

		public static Task Run(string[] args)
		{
			var host = CreateHostBuilder(args).Build();

			var hostApplicationLifetime = host.Services.GetService<IHostApplicationLifetime>();
			hostApplicationLifetime.ApplicationStopping.Register(OnApplicationStopping);

			_telegramBotService = host.Services.GetService<ITelegramBotService>();
			_jobManagerHelper = host.Services.GetService<IJobManagerHelper>();

			_telegramBotService.Start();
			_jobManagerHelper.ScheduleJobs();

			return host.RunAsync();
		}

		public static IHostBuilder CreateHostBuilder(string[] args)
		{
			return Host
				.CreateDefaultBuilder(args)
				.ConfigureServices(ConfigureServices);
		}

		private static void ConfigureServices(
			HostBuilderContext hostBuilderContext,
			IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddDbContext<ApplicationDbContext>(s =>
				{
					s.UseSqlite("Data Source=game.db");
					s.ConfigureWarnings(wcb => wcb.Log(
						(RelationalEventId.CommandExecuting, LogLevel.Debug),
						(RelationalEventId.CommandExecuted, LogLevel.Debug)));
				})
				.AddTelegramBotClient()
				.AddInjectedServices();
		}

		private static void OnApplicationStopping()
		{
			_telegramBotService.Stop();
			_jobManagerHelper.Stop();
		}
	}
}
