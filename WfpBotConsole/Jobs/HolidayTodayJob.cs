using FluentScheduler;
using System;
using System.Linq;
using Telegram.Bot;
using WfpBotConsole.DB;
using HtmlAgilityPack;
using System.Net;
using Telegram.Bot.Types.Enums;

namespace WfpBotConsole.Jobs
{
	class HolidayTodayJob : IJob
	{
		private readonly GameRepository _repository;
		private readonly ITelegramBotClient _client;

		private const string DateFormat = "dddd, dd MMMM";

		public HolidayTodayJob(GameRepository repository, ITelegramBotClient client)
		{
			_repository = repository;
			_client = client;
		}

		public async void Execute()
		{
			try
			{
				using var webClient = new WebClient();
				var html = await webClient.DownloadStringTaskAsync("https://kakoysegodnyaprazdnik.com/");

				var doc = new HtmlDocument();
				doc.LoadHtml(html);

				var holidays = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'first')]/li[contains(@class, 'block1')]").Select(li => "_" + li.InnerText + "_");

				if (holidays.Any())
				{
					var todayFormatted = DateTime.Today.ToString(DateFormat, new System.Globalization.CultureInfo("ru-RU")).ReplaceDigits();

					var message = Messages.TodayString
							+ todayFormatted
							+ Environment.NewLine
							+ Environment.NewLine
							+ Messages.TodayHolidays
							+ Environment.NewLine
							+ string.Join(Environment.NewLine, holidays);


					var allChatIds = await _repository.GetAllChatsIds();

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
	}
}
