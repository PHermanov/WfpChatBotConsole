using System.Threading.Tasks;

namespace WfpBotConsole.Services
{
	public interface ITelegramBotService
	{
		Task Start();

		void Stop();
	}
}