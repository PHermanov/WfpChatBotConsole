using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
	[Inject]
	public class HelpCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;

		public string CommandKey => "/help";

		public HelpCommand(ITelegramBotClient telegramBotClient)
		{
			_telegramBotClient = telegramBotClient;
		}

		public async Task Execute(long chatId)
		{
			await _telegramBotClient.TrySendTextMessageAsync(chatId, Messages.Help);
		}
	}
}
