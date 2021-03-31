using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Jobs;
using WfpBotConsole.Services.News;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Commands
{
	[Inject]
	public class TestCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;
		private readonly IEnumerable<INewsService> _newsServices;

		public string CommandKey => "/iddqd";

		public TestCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository,
			IEnumerable<INewsService> newsServices)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
			_newsServices = newsServices;

		}

		public async Task Execute(long chatId)
		{
			//await _telegramBotClient.TrySendTextMessageAsync(chatId, $"Хуест!", ParseMode.Markdown);
			//await _telegramBotClient.TrySendStickerAsync(chatId, StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Yoba));

			//var monthJob = new MonthWinnerJob(_gameRepository, _telegramBotClient);
			//await monthJob.Execute(chatId);

            var aprilJob = new FoolsDayJob(_gameRepository, _telegramBotClient);
            await aprilJob.Execute(chatId);
        }
	}
}
