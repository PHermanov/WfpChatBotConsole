using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WfpBotConsole.Bootstrap;
using WfpBotConsole.Jobs;
using WfpBotConsole.Services;

namespace WfpBotConsole
{
	public class ApplicationHost
	{
		private ITelegramBotService _telegramBotService;

		public Task Run()
		{
			var host = CreateHostBuilder().Build();

			var hostApplicationLifetime = host.Services.GetService<IHostApplicationLifetime>();
			hostApplicationLifetime.ApplicationStopping.Register(OnApplicationStopping);

			_telegramBotService = host.Services.GetService<ITelegramBotService>();
			var jobManagerHelper = host.Services.GetService<IJobManagerHelper>();

			_telegramBotService.Start();
			jobManagerHelper.ScheduleJobs();

			return host.RunAsync();
		}

		private IHostBuilder CreateHostBuilder()
		{
			return Host
				.CreateDefaultBuilder()
				.ConfigureServices(ConfigureServices);
		}

		private void ConfigureServices(
			HostBuilderContext hostBuilderContext,
			IServiceCollection serviceCollection)
		{
			serviceCollection
				.AddTelegramBotClient()
				.AddInjectedServices();
		}

		private void OnApplicationStopping()
		{
			_telegramBotService.Stop();
		}
	}
}
