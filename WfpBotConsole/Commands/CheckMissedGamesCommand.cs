using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Data;
using WfpBotConsole.Models;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
	[Inject]
	public class CheckMissedGamesCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/checkmissedgames";

		public CheckMissedGamesCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var lastGame = await _gameRepository.GetLastPlayedGameAsync(chatId);

			if (lastGame != null)
			{
				var gameDate = lastGame.PlayedAt.AddDays(1);
				var results = new List<string>();

				while (gameDate.Date < DateTime.Today)
				{
					var users = await _gameRepository.GetAllPlayersAsync(chatId);

					var newWinner = users[new Random().Next(users.Count)];

					await _gameRepository.SaveGameResultAsync(new GameResult()
					{
						ChatId = chatId,
						UserId = newWinner.UserId,
						UserName = newWinner.UserName,
						PlayedAt = gameDate
					});

					results.Add($"*{gameDate:d}* - {newWinner.GetUserMention()}");

					gameDate = gameDate.AddDays(1);
				}

				if (results.Any())
				{
					var msg = string.Format(Messages.MissedGames, $"{Environment.NewLine}{string.Join(Environment.NewLine, results)}");
					await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
				}
			}
		}
	}
}
