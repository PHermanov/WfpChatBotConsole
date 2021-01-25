using System.Threading.Tasks;

namespace WfpBotConsole.Services
{
	public interface ICommandsService
	{
		Task Execute(long chatId, string commandKey);
	}
}