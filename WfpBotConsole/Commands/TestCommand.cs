using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class TestCommand : Command
    {
        public override async Task Execute(Message message, ITelegramBotClient client, GameRepository repository = null)
        {
            var chatId = message.Chat.Id;

            await client.SendTextMessageAsync(chatId, $"Хуест! Лососни, _{ message.From.Username }_!", parseMode: ParseMode.Markdown);
        }
    }
}
