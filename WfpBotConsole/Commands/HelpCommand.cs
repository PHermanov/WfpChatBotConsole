using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class HelpCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository = null)
        {
            await client.SendTextMessageAsync(chatId, "Хелп-хуелп!", parseMode: ParseMode.Markdown);
        }
    }
}
