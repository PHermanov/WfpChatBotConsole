﻿using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;
using WfpBotConsole.Bootstrap;
using WfpBotConsole.DB;
using WfpBotConsole.Jobs;
using WfpBotConsole.Services;

namespace WfpBotConsole
{
	public class ApplicationHost
	{
		private ITelegramBotService _telegramBotService;
		private IJobManagerHelper _jobManagerHelper;

		public Task Run()
		{
			var host = CreateHostBuilder().Build();

			var hostApplicationLifetime = host.Services.GetService<IHostApplicationLifetime>();
			hostApplicationLifetime.ApplicationStopping.Register(OnApplicationStopping);

			_telegramBotService = host.Services.GetService<ITelegramBotService>();
			_jobManagerHelper = host.Services.GetService<IJobManagerHelper>();

			_telegramBotService.Start();
			_jobManagerHelper.ScheduleJobs();

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
				.AddDataBase()
				.AddTelegramBotClient()
				.AddInjectedServices();
		}

		private void OnApplicationStopping()
		{
			_telegramBotService.Stop();
			_jobManagerHelper.Stop();
		}
	}
}
