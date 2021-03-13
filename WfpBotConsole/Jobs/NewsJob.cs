using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Data;
using WfpBotConsole.Services.News;

namespace WfpBotConsole.Jobs
{
	[Inject]
	public class NewsJob : IScheduleJob
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;
		private readonly IEnumerable<INewsService> _newsServices;

		public NewsJob(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository,
			IEnumerable<INewsService> newsServices)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
			_newsServices = newsServices;
		}

		public async void Execute()
		{
			Console.ForegroundColor = ConsoleColor.Yellow;

			Console.WriteLine("Executing NewsJob");

			var allChatIds = await _gameRepository.GetAllChatsIdsAsync();

			Console.WriteLine("IDs: " + string.Join(',', allChatIds));

			await Execute(allChatIds);

			Console.ForegroundColor = ConsoleColor.Gray;
		}

		public async Task Execute(params long[] chatIds)
		{
			try
			{
				var news = new List<string>();

				foreach (var newsService in _newsServices)
				{
					news.AddRange(await newsService.GetNewsAsync());
				}

				if (!news.Any())
				{
					return;
				}

				for (int i = 0; i < chatIds.Length; i++)
				{
					foreach (var line in news)
					{
						Console.WriteLine("Sending " + line + " to Chat: " + chatIds[i]);

						await _telegramBotClient.TrySendTextMessageAsync(chatIds[i], line, ParseMode.Markdown);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.GetType());
				Console.WriteLine(ex.Message);
			}
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.WithName(nameof(NewsJob)).ToRunEvery(0).Hours().At(0).Between(9, 0, 21, 0));
		}
	}
}
