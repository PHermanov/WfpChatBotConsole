using FluentScheduler;
using System;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
	public class MonthWinnerJob : IJob
	{
		private readonly GameRepository _repository;
		private readonly ITelegramBotClient _client;

		public MonthWinnerJob(GameRepository repository, ITelegramBotClient client)
		{
			_repository = repository;
			_client = client;
		}

		public async void Execute()
		{
			var allChatIds = await _repository.GetAllChatsIdsAsync();

			for (int i = 0; i < allChatIds.Length; i++)
			{
				var monthWinner = await _repository.GetWinnerForMonthAsync(allChatIds[i], DateTime.Today);

				if (monthWinner != null)
				{
					var mention = monthWinner.GetUserMention();
					var message = "";
				
					await _client.TrySendTextMessageAsync(allChatIds[i], message, ParseMode.Markdown);
				}
			}
		}
	}
}
