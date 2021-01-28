using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Services
{
	public class AutoReplyService : IAutoReplyService
	{
		private readonly ITelegramBotClient _telegramBotClient;

		public AutoReplyService(ITelegramBotClient telegramBotClient)
		{
			_telegramBotClient = telegramBotClient;
		}

		public async Task AutoReplyAsync(Message message)
		{
			var answer = AutoReply.ResourceManager.GetString(message.Text.Trim().ToLower());

			if (!string.IsNullOrEmpty(answer))
			{
				await Task.Delay(TimeSpan.FromSeconds(1));
				await _telegramBotClient.TrySendTextMessageAsync(message.Chat.Id, answer, ParseMode.Markdown, replyToMessageId: message.MessageId);
			}
		}
	}
}
