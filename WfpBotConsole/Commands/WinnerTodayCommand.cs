using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class WinnerTodayCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository)
        {
            var todayResult = await repository.GetTodayResultAsync(chatId);

            if (todayResult != null)
            {
                await client.TrySendTextMessageAsync(chatId, string.Format(Messages.TodayWinnerAlreadySet, todayResult.GetUserMention()), ParseMode.Markdown);
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
