using System.Threading.Tasks;

namespace WfpBotConsole.Commands
{
	public interface ICommand
	{
		string CommandKey { get; }

		Task Execute(long chatId);
	}
}
