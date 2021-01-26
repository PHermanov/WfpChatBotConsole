using FluentScheduler;
using System.Threading.Tasks;

namespace WfpBotConsole.Jobs
{
	public interface IScheduleJob : IJob
	{
		void Schedule();

		Task Execute(params long[] chatIds);
	}
}
