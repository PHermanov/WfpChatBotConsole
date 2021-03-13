using System;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Data;
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

			_telegramBotClient.OnMessage += OnMessage;
			_telegramBotClient.OnReceiveError += OnReceiveError;
			_telegramBotClient.OnReceiveGeneralError += OnReceiveGeneralError;
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

		private void OnReceiveError(object sender, ReceiveErrorEventArgs e)
		{
			Console.WriteLine(nameof(OnReceiveError));
			Console.WriteLine(e.ApiRequestException.Message);
		}

		private void OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
		{
			Console.WriteLine(nameof(OnReceiveGeneralError));
			Console.WriteLine(e.Exception.Message);
		}

		private async void OnMessage(object sender, MessageEventArgs e)
		{
			if (!e.Message.From.IsBot)
			{
				switch (e.Message.Type)
				{
					case MessageType.Text:
						{
							await ProcessTextMessage(sender, e);
							break;
						}
					case MessageType.ChatMembersAdded:
						{
							await ProcessChatMembersAddedMessage(sender, e);
							break;
						}
					case MessageType.ChatMemberLeft:
						{
							await ProcessChatMemberLeftMessage(sender, e);
							break;
						}
				}
			}
		}

		private async Task ProcessTextMessage(object sender, MessageEventArgs e)
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
				Console.WriteLine(string.Format(Messages.NewPlayerAdded, userName));
			}

			await _autoReplyService.AutoReplyAsync(e.Message);

			await _autoReplyService.AutoMentionAsync(e.Message);

			if (text.StartsWith(@"/"))
			{
				await _commandsService.Execute(e.Message.Chat.Id, text);
			}
		}

		private async Task ProcessChatMembersAddedMessage(object sender, MessageEventArgs e)
		{
			if (sender is TelegramBotClient tbc)
			{
				if (e.Message.NewChatMembers.Any(user => user.Id == tbc.BotId))
				{
					// Save new chat info / or restore from archive (check all existing users)
					// Save requestor user info
					// Save all other new users from message (except bots)
					// Save chat admins
				}
				else
				{
					// Save new users from message 
					// Greetings
				}
			}

			await Task.CompletedTask;
		}

		private async Task ProcessChatMemberLeftMessage(object sender, MessageEventArgs e)
		{
			if (sender is TelegramBotClient tbc)
			{
				if (e.Message.LeftChatMember.Id == tbc.BotId)
				{
					// Archive chat info
				}
				else
				{
					// Archive left user
				}
			}

			await Task.CompletedTask;
		}
	}
}
