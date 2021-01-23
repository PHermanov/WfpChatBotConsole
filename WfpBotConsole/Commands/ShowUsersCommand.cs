using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public class ShowUsersCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, IGameRepository repository)
        {
            var users = await repository.GetAllPlayersAsync(chatId);

            string msg = string.Join(';', users);

            await client.TrySendTextMessageAsync(chatId, msg);
        }
    }
}
