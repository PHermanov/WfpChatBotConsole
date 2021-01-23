using System.Threading.Tasks;

namespace WfpBotConsole
{
	class Program
	{
		static Task Main(string[] args)
		{
			return new ApplicationHost().Run();
		}
	}
}
