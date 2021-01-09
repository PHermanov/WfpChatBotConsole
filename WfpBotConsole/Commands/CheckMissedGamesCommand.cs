using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Models;

namespace WfpBotConsole.Commands
{
	public class CheckMissedGamesCommand : Command
	{
		public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository = null)
		{
			var lastGame = await repository.GetLastPlayedGame(chatId);

			if (lastGame != null)
			{
				var gameDate = lastGame.PlayedAt.AddDays(1);
				var results = new List<string>();

				while (gameDate.Date != DateTime.Today)
				{
					var users = await repository.GetAllPlayersAsync(chatId);

					var newWinner = users[new Random().Next(users.Count)];

					await repository.SaveGameResult(new GameResult()
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
					await client.SendTextMessageAsync(chatId, msg, ParseMode.Markdown);
				}
			}
		}
	}
}
