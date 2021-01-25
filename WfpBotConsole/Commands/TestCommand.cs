using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	public class TestCommand : Command
	{
		public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository = null)
		{
			await client.TrySendTextMessageAsync(chatId, $"Хуест!", parseMode: ParseMode.Markdown);
		}
	}
}
