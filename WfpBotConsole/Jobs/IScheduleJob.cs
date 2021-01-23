using FluentScheduler;

namespace WfpBotConsole.Jobs
{
	public interface IScheduleJob : IJob
	{
		void Schedule();
	}
}
