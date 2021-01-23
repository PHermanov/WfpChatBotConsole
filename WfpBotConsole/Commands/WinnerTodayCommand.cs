using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	public class WinnerTodayCommand : Command
	{
		public enum Language
		{
			Ru,
			Ukr
		}

		private Language Lang { get; set; } = Language.Ru;

		public WinnerTodayCommand(Language lang)
		{
			Lang = lang;
		}

		public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository)
		{
			var todayResult = await repository.GetTodayResultAsync(chatId);

			if (todayResult != null)
			{
				var message = Lang switch
				{
					Language.Ru => Messages.TodayWinnerAlreadySet,
					Language.Ukr => Messages.TodayWinnerAlreadySetUkr,
					_ => throw new System.NotImplementedException()
				};

				await client.TrySendTextMessageAsync(chatId, string.Format(message, todayResult.GetUserMention()), ParseMode.Markdown);
			}
			else
			{
				await client.TrySendTextMessageAsync(chatId, Messages.WinnerNotSetYet, ParseMode.Markdown);

				var yesterdayResult = await repository.GetYesterdayResultAsync(chatId);

				if (yesterdayResult != null)
				{
					await client.TrySendTextMessageAsync(chatId, string.Format(Messages.YesterdayWinner, yesterdayResult.GetUserMention()), ParseMode.Markdown);
				}
			}

		}
	}
}
