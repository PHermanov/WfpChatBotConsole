using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	public class CurrentMonthTopWinnersCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/month";

		public CurrentMonthTopWinnersCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			int top = 5;

			var winners = await _gameRepository.GetTopWinnersForMonthAsync(chatId, top, DateTime.Today);

			string msg = string.Format(Messages.TopMonthWinners, top) + Environment.NewLine
				+ string.Join(Environment.NewLine, winners);

			await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
		}
	}
}
