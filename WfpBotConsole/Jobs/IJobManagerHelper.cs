namespace WfpBotConsole.Jobs
{
	public interface IJobManagerHelper
	{
		void ScheduleJobs();

		void Stop();
	}
}