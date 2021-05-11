using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
    [Inject]
    public class WinnerTomorrowCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;

        public string CommandKey => "/tomorrow";

        public WinnerTomorrowCommand(ITelegramBotClient telegramBotClient)
        {
            _telegramBotClient = telegramBotClient;
        }

        public async Task Execute(long chatId)
        {
            await _telegramBotClient.TrySendTextMessageAsync(chatId, Messages.Tomorrow, ParseMode.Markdown);
        }
    }
}
