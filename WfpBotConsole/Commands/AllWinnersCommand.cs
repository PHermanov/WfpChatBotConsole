using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	public class AllWinnersCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository)
        {
            var allWinners = await repository.GetAllWinnersAsync(chatId);

            if (allWinners.Any())
            {
                string msg = Messages.AllWinners 
                    + Environment.NewLine 
                    + Environment.NewLine
                    + string.Join(Environment.NewLine, allWinners);

                await client.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
            }
        }
    }
}
