using System.Threading.Tasks;
using Telegram.Bot.Types;

namespace WfpBotConsole.Services
{
	public interface IAutoReplyService
	{
		Task AutoReplyAsync(Message message);

		Task AutoMentionAsync(Message message);
	}
}