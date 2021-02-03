using FluentScheduler;
using Flurl.Http;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Jobs
{
	[Inject]
	class HolidayTodayJob : IScheduleJob
	{
		private readonly IGameRepository _repository;
		private readonly ITelegramBotClient _client;

		private const string DateFormat = "dddd, dd MMMM";

		public HolidayTodayJob(IGameRepository repository, ITelegramBotClient client)
		{
			_repository = repository;
			_client = client;
		}

		public async void Execute()
		{
			var allChatIds = await _repository.GetAllChatsIdsAsync();

			await Execute(allChatIds);
		}

		public async Task Execute(params long[] chatIds)
		{
			try
			{
				var holidays = await GetHolidays();

				if (holidays.Any())
				{
					var todayFormatted = DateTime.Today.ToString(DateFormat, new System.Globalization.CultureInfo("ru-RU")).ReplaceDigits();

					var message = Messages.TodayString
							+ todayFormatted
							+ Environment.NewLine
							+ Environment.NewLine
							+ string.Join(Environment.NewLine, holidays);


					for (int i = 0; i < chatIds.Length; i++)
					{
						await _client.TrySendTextMessageAsync(chatIds[i], message, ParseMode.Markdown);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.GetType());
				Console.WriteLine(ex.Message);
			}
		}

		private async Task<IEnumerable<string>> GetHolidays()
		{
			var html = await "https://kakoysegodnyaprazdnik.com/".GetStringAsync();

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			return doc.DocumentNode
				.SelectNodes("//ul[contains(@class, 'first')]/li[contains(@class, 'block1')]")
				.Select(li => $"\u25AA _{li.InnerText}_");
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.WithName(nameof(HolidayTodayJob)).ToRunEvery(0).Days().At(10, 30));
		}
	}
}
