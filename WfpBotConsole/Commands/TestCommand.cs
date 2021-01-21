using HtmlAgilityPack;
using System;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Jobs;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Commands
{
	public class TestCommand : Command
	{
		public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository = null)
		{
			await client.TrySendTextMessageAsync(chatId, $"Хуест!", parseMode: ParseMode.Markdown);
			// await client.TrySendStickerAsync(chatId, StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Frog));

			using var webClient = new WebClient();
			var html = await webClient.DownloadStringTaskAsync("https://kakoysegodnyaprazdnik.com/");

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			var holidays = doc.DocumentNode.SelectNodes("//ul[contains(@class, 'first')]/li[contains(@class, 'block1')]").Select(li => "_" + li.InnerText + "_");

			var culture = new System.Globalization.CultureInfo("ru-RU");

			var todayDayName = culture.DateTimeFormat.GetDayName(DateTime.Today.DayOfWeek);
			var todayFormatted = DateTime.Today.ToString(culture.DateTimeFormat.LongDatePattern);

			var message = string.Format(Messages.TodayString, todayDayName, todayFormatted)
					+ Environment.NewLine
					+ Messages.TodayHolidays
					+ Environment.NewLine
					+ string.Join(Environment.NewLine, holidays);

			if (holidays.Any())
			{
				await client.TrySendTextMessageAsync(chatId, message);
			}
		}
	}
}
