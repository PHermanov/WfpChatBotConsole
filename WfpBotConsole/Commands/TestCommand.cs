using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Jobs;

namespace WfpBotConsole.Commands
{
	public class TestCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/test";

		public TestCommand(ITelegramBotClient telegramBotClient, IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			await _telegramBotClient.TrySendTextMessageAsync(chatId, $"Хуест!", ParseMode.Markdown);

			//var monthJob = new MonthWinnerJob(_gameRepository, _telegramBotClient);
			//await monthJob.Execute(chatId);
		}
	}
}
