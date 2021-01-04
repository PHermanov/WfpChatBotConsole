using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class HelpCommand : Command
    {
        public override async Task Execute(Message message, ITelegramBotClient client, GameRepository repository = null)
        {
            var chatId = message.Chat.Id;

            await client.SendTextMessageAsync(chatId, "Хелп-хуелп!", parseMode: ParseMode.Markdown);
        }
    }
}
