using FluentScheduler;
using System.Collections.Generic;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;

namespace WfpBotConsole.Jobs
{
	[Inject(RegistrationScope.Singleton)]
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
