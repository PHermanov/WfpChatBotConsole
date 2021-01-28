using System;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Services
{
	[Inject(RegistrationScope.Singleton)]
	public class AutoReplyService : IAutoReplyService
	{
		private readonly ITelegramBotClient _telegramBotClient;

		public AutoReplyService(ITelegramBotClient telegramBotClient)
		{
			_telegramBotClient = telegramBotClient;
		}

		public async Task AutoReplyAsync(Message message)
		{
			var preparedText = new Regex("[ !?]")
				.Replace(message.Text, string.Empty)
				.ToLower();

			var answer = AutoReply.ResourceManager.GetString(preparedText);

			if (!string.IsNullOrEmpty(answer))
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				await _telegramBotClient.TrySendTextMessageAsync(message.Chat.Id, answer, ParseMode.Markdown, replyToMessageId: message.MessageId);
			}
		}
	}
}
