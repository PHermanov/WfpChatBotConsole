using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;

namespace WfpBotConsole.Jobs
{
	[Inject]
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

			Console.WriteLine(string.Join(Environment.NewLine, JobManager.AllSchedules.Select(s => $"Job: {s.Name} Next run: {s.NextRun:G}")));
		}

		public void Stop()
		{
			JobManager.StopAndBlock();
		}
	}
}
