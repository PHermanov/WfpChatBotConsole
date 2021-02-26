using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Services
{
	[Inject]
	public class TelegramBotService : ITelegramBotService
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;
		private readonly ICommandsService _commandsService;
		private readonly IAutoReplyService _autoReplyService;

		public TelegramBotService(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository,
			ICommandsService commandsService,
			IAutoReplyService autoReplyService)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
			_commandsService = commandsService;
			_autoReplyService = autoReplyService;

			_telegramBotClient.OnMessage += Bot_OnMessage;
			_telegramBotClient.OnReceiveError += Client_OnReceiveError;
			_telegramBotClient.OnReceiveGeneralError += Client_OnReceiveGeneralError;
		}

		public async Task Start()
		{
			var me = await _telegramBotClient.GetMeAsync();
			Console.WriteLine($"Bot started {me.Id} : {me.FirstName}");

			_telegramBotClient.StartReceiving();
		}

		public void Stop()
		{
			_telegramBotClient.StopReceiving();
		}

		private void Client_OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
		{
			Console.WriteLine(nameof(Client_OnReceiveGeneralError));
			Console.WriteLine(e.Exception.Message);
		}

		private void Client_OnReceiveError(object sender, ReceiveErrorEventArgs e)
		{
			Console.WriteLine(nameof(Client_OnReceiveError));
			Console.WriteLine(e.ApiRequestException.Message);
		}

		private async void Bot_OnMessage(object sender, MessageEventArgs e)
		{
			if (!e.Message.From.IsBot && e.Message.Type == MessageType.Text && !string.IsNullOrWhiteSpace(e.Message.Text))
			{
				var chatId = e.Message.Chat.Id;
				var userName = e.Message.From.Username;
				var text = e.Message.Text;

				Console.WriteLine($"Received a message in chat {chatId}. {userName} : {text}");

				if (string.IsNullOrWhiteSpace(userName))
				{
					userName = e.Message.From.FirstName + " " + e.Message.From.LastName;
				}

				var newPlayer = await _gameRepository.CheckPlayerAsync(chatId, e.Message.From.Id, userName);

				if (newPlayer)
				{
					// await _telegramBotClient.TrySendTextMessageAsync(chatId, string.Format(Messages.NewPlayerAdded, name));
					Console.WriteLine(string.Format(Messages.NewPlayerAdded, userName));
				}

				await _autoReplyService.AutoReplyAsync(e.Message);

				await _autoReplyService.AutoMentionAsync(e.Message);

				if (text.StartsWith(@"/"))
				{
					await _commandsService.Execute(e.Message.Chat.Id, text);
				}
			}
		}
	}
}
