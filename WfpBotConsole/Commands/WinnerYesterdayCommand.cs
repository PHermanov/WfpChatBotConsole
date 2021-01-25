﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	public class WinnerYesterdayCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/yesterday";

		public WinnerYesterdayCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var yesterdayResult = await _gameRepository.GetYesterdayResultAsync(chatId);

			if (yesterdayResult != null)
			{
				var msg = string.Format(Messages.YesterdayWinner, yesterdayResult.GetUserMention());

				await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
				await _telegramBotClient.TrySendTextMessageAsync(chatId, Messages.WinnerForever, ParseMode.Markdown);
			}
		}
	}
}
