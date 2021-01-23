using Microsoft.Extensions.DependencyInjection;
using WfpBotConsole.Jobs;

namespace WfpBotConsole.Bootstrap
{
	public static class JobManagerBootstrap
	{
		public static IServiceCollection AddJobs(this IServiceCollection serviceCollection)
		{
			serviceCollection.AddTransient<IScheduleJob, GetWinnerEverydayJob>();
			serviceCollection.AddTransient<IScheduleJob, WednesdayJob>();
			serviceCollection.AddTransient<IScheduleJob, HolidayTodayJob>();
			serviceCollection.AddTransient<IScheduleJob, MonthWinnerJob>();

			serviceCollection.AddSingleton<IJobManagerHelper, JobManagerHelper>();

			return serviceCollection;
		}
	}
}
