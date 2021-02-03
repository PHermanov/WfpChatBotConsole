using FluentScheduler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
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
			var allChatIds = await _gameRepository.GetAllChatsIdsAsync();

			await Execute(allChatIds);
		}

		public async Task Execute(params long[] chatIds)
		{
			try
			{
				var news = _newsServices
					.Select(async (s) => await s.GetNewsAsync())
					.SelectMany(s => s.Result);

				for (int i = 0; i < chatIds.Length; i++)
				{
					foreach (var line in news)
					{
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
			JobManager.AddJob(this, s => s.ToRunEvery(0).Hours().Between(9, 0, 21, 0));
		}
	}
}
