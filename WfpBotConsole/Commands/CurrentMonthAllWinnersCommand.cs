using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Commands
{
    [Inject]
    public class CurrentMonthAllWinnersCommand : ICommand
    {
        private readonly ITelegramBotClient _telegramBotClient;
        private readonly IGameRepository _gameRepository;

        public string CommandKey => "/month";

        public CurrentMonthAllWinnersCommand(
            ITelegramBotClient telegramBotClient,
            IGameRepository gameRepository)
        {
            _telegramBotClient = telegramBotClient;
            _gameRepository = gameRepository;
        }

        public async Task Execute(long chatId)
        {
            var winners = await _gameRepository.GetAllWinnersForMonthAsync(chatId, DateTime.Today);

            string msg = Messages.AllMonthWinners + Environment.NewLine
                + string.Join(Environment.NewLine, winners);

            await _telegramBotClient.TrySendTextMessageAsync(chatId, msg, ParseMode.Html);
        }
    }
}
