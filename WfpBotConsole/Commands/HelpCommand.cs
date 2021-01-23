using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class HelpCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository = null)
        {
            await client.TrySendTextMessageAsync(chatId, Messages.Help);
        }
    }
}
