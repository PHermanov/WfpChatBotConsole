using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
	public class AllWinnersCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/all";

		public AllWinnersCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var allWinners = await _gameRepository.GetAllWinnersAsync(chatId);

			if (allWinners.Any())
			{
				string msg = Messages.AllWinners
					+ Environment.NewLine
					+ Environment.NewLine
					+ string.Join(Environment.NewLine, allWinners);

				await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
			}
		}
	}
}
