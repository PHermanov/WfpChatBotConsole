using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class CurrentMonthTopWinnersCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository)
        {
            int top = 5;

            var winners = await repository.GetTopWinnersForMonthAsync(chatId, top, DateTime.Today);

            string msg = string.Format(Messages.TopMonthWinners, top) + Environment.NewLine
                + string.Join(Environment.NewLine, winners);

            await client.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
        }
    }
}
