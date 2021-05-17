using System;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Services
{
	[Inject]
	public class AutoReplyService : IAutoReplyService
	{
		private readonly ITelegramBotClient _telegramBotClient;
		private readonly IGameRepository _gameRepository;

		public AutoReplyService(
			ITelegramBotClient telegramBotClient,
			IGameRepository gameRepository)
		{
			_telegramBotClient = telegramBotClient;
			_gameRepository = gameRepository;
		}

		public async Task AutoReplyAsync(Message message)
		{
			var preparedText = new Regex("[ !?]")
				.Replace(message.Text, string.Empty)
				.ToLower();

			var answer = AutoReply.ResourceManager.GetString(preparedText);

			if (!string.IsNullOrEmpty(answer))
			{
				// multiple answers, pick random
                if (answer.Contains(";"))
                {
                    var answers = answer.Split(new[] {';'}, StringSplitOptions.RemoveEmptyEntries);
                    answer = answers[new Random().Next(answers.Length)];
                }

				await Task.Delay(TimeSpan.FromSeconds(1));
				await _telegramBotClient.TrySendTextMessageAsync(message.Chat.Id, answer, ParseMode.Markdown, replyToMessageId: message.MessageId);
			}
		}

		public async Task AutoMentionAsync(Message message)
		{
			var splitText = message.Text?.Split(new[] { " " }, StringSplitOptions.RemoveEmptyEntries);

			if (splitText.Any(w => w.Equals("@all", StringComparison.OrdinalIgnoreCase)))
			{
				var users = (await _gameRepository
					.GetAllPlayersAsync(message.Chat.Id))
					.Where(user => user.UserId != message.From.Id);

				if (users.Any())
				{
					var answer = $"\U0001F51D {users.GetUsersMention()}";

					await Task.Delay(TimeSpan.FromSeconds(1));
					await _telegramBotClient.TrySendTextMessageAsync(message.Chat.Id, answer, ParseMode.Markdown, replyToMessageId: message.MessageId);
				}
			}
		}
	}
}
