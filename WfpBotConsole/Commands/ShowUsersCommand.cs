using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
	[Inject]
	public class ShowUsersCommand : ICommand
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public string CommandKey => "/showusers";

		public ShowUsersCommand(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task Execute(long chatId)
		{
			var users = await _gameRepository.GetAllPlayersAsync(chatId);

			string msg = string.Join(';', users);

			await _telegramBotClient.TrySendTextMessageAsync(chatId, msg);
		}
	}
}
