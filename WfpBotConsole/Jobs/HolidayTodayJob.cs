using FluentScheduler;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
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


					var allChatIds = await _repository.GetAllChatsIdsAsync();

					for (int i = 0; i < allChatIds.Length; i++)
					{
						await _client.TrySendTextMessageAsync(allChatIds[i], message, ParseMode.Markdown);
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
			using var webClient = new WebClient();
			var html = await webClient.DownloadStringTaskAsync("https://kakoysegodnyaprazdnik.com/");

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			return doc.DocumentNode
				.SelectNodes("//ul[contains(@class, 'first')]/li[contains(@class, 'block1')]")
				.Select(li => $"\u25AA _{li.InnerText}_");
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Days().At(10, 30));
		}
	}
}
