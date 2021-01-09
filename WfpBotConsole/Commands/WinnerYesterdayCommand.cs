using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class WinnerYesterdayCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository)
        {
            var yesterdayResult = await repository.GetYesterdayResultAsync(chatId);

            if (yesterdayResult != null)
            {
                var msg = string.Format(Messages.YesterdayWinner, yesterdayResult.GetUserMention());

                await client.SendTextMessageAsync(chatId, msg, ParseMode.Markdown);
                await client.SendTextMessageAsync(chatId, Messages.WinnerForever, ParseMode.Markdown);
            }
        }
    }
}
