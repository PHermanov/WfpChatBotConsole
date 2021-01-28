using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Models;
using WfpBotConsole.Resources;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Commands
{
	[Inject]
	public class NewWinnerCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/newwinner";

		public NewWinnerCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var todayResult = await _gameRepository.GetTodayResultAsync(chatId);

			var messageTemplate = Messages.TodayWinnerAlreadySet;

			if (todayResult == null)
			{
				var users = await _gameRepository.GetAllPlayersAsync(chatId);

				var newWinner = users[new Random().Next(users.Count)];

				messageTemplate = Messages.NewWinner;

				todayResult = new GameResult()
				{
					ChatId = chatId,
					UserId = newWinner.UserId,
					UserName = newWinner.UserName,
					PlayedAt = DateTime.Today
				};

				await _gameRepository.SaveGameResultAsync(todayResult);
			}

			var msg = string.Format(messageTemplate, todayResult.GetUserMention());

			await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
			await _telegramBotClient.TrySendStickerAsync(chatId, StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Yoba));
		}
	}
}
