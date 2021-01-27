using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
	public class WinnerTodayRuCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/today";

		public WinnerTodayRuCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var todayResult = await _gameRepository.GetTodayResultAsync(chatId);

			if (todayResult != null)
			{
				await _telegramBotClient.TrySendTextMessageAsync(chatId, string.Format(Messages.TodayWinnerAlreadySet, todayResult.GetUserMention()), ParseMode.Markdown);
			}
			else
			{
				await _telegramBotClient.TrySendTextMessageAsync(chatId, Messages.WinnerNotSetYet, ParseMode.Markdown);

				var yesterdayResult = await _gameRepository.GetYesterdayResultAsync(chatId);

				if (yesterdayResult != null)
				{
					await _telegramBotClient.TrySendTextMessageAsync(chatId, string.Format(Messages.YesterdayWinner, yesterdayResult.GetUserMention()), ParseMode.Markdown);
				}
			}
		}
	}
}
