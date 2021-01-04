using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class CurrentMonthTopWinnersCommand : Command
    {
        public override async Task Execute(Message message, ITelegramBotClient client, GameRepository repository)
        {
            var chatId = message.Chat.Id;

            int top = 5;

            var winners = await repository.GetTopWinnersForMonth(chatId, top, DateTime.Today);

            string msg = string.Format(Messages.TopMonthWinners, top) + Environment.NewLine 
                + string.Join(Environment.NewLine, winners);

            await client.SendTextMessageAsync(chatId, msg, ParseMode.Markdown);
        }
    }
}
