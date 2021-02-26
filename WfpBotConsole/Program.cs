using Microsoft.Extensions.Hosting;
using System.Threading.Tasks;

namespace WfpBotConsole
{
	class Program
	{
		static Task Main(string[] args)
		{
			return ApplicationHost.Run(args);
		}

		static IHostBuilder CreateHostBuilder(string[] args)
		{
			return ApplicationHost.CreateHostBuilder(args);
		}
	}
}
