using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class ShowUsersCommand : Command
    {
        public override async Task Execute(Message message, ITelegramBotClient client, GameRepository repository)
        {
            var chatId = message.Chat.Id;

            var users = await repository.GetAllPlayersAsync(chatId);

            string msg = string.Join(';', users);

            await client.SendTextMessageAsync(chatId, msg);
        }
    }
}
