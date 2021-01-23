using FluentScheduler;
using System.Collections.Generic;

namespace WfpBotConsole.Jobs
{
	public class JobManagerHelper : IJobManagerHelper
	{
		private readonly IEnumerable<IScheduleJob> _jobs;

		public JobManagerHelper(IEnumerable<IScheduleJob> jobs)
		{
			_jobs = jobs;
		}

		public void ScheduleJobs()
		{
			foreach (var job in _jobs)
			{
				job.Schedule();
			}

			JobManager.Initialize();
		}
	}
}
