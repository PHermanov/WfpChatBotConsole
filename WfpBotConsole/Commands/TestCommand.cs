using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;

namespace WfpBotConsole.Commands
{
	public class TestCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;

		public string CommandKey => "/test";

		public TestCommand(ITelegramBotClient telegramBotClient)
		{
			_telegramBotClient = telegramBotClient;
		}

		public async Task Execute(long chatId)
		{
			await _telegramBotClient.TrySendTextMessageAsync(chatId, $"Хуест!", parseMode: ParseMode.Markdown);
		}
	}
}
