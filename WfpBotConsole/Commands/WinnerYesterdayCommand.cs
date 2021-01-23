using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class WinnerYesterdayCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository)
        {
            var yesterdayResult = await repository.GetYesterdayResultAsync(chatId);

            if (yesterdayResult != null)
            {
                var msg = string.Format(Messages.YesterdayWinner, yesterdayResult.GetUserMention());

                await client.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
                await client.TrySendTextMessageAsync(chatId, Messages.WinnerForever, ParseMode.Markdown);
            }
        }
    }
}
