using System;
using System.Threading;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using Telegram.Bot.Types.ReplyMarkups;
using WfpBotConsole.Models;

namespace WfpBotConsole
{
	public static class Extensions
	{
		public static async Task TrySendTextMessageAsync(this ITelegramBotClient client, ChatId chatId, string text, ParseMode parseMode = ParseMode.Default, bool disableWebPagePreview = false, bool disableNotification = false, int replyToMessageId = 0, IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default)
		{
			try
			{
				await client.SendTextMessageAsync(chatId, text, parseMode, disableWebPagePreview, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.GetType());
				Console.WriteLine(exception.Message);
			}
		}

		public static async Task TrySendStickerAsync(this ITelegramBotClient client, ChatId chatId, InputOnlineFile sticker, bool disableNotification = false, int replyToMessageId = 0, IReplyMarkup replyMarkup = null, CancellationToken cancellationToken = default)
		{
			try
			{
				await client.SendStickerAsync(chatId, sticker, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.GetType());
				Console.WriteLine(exception.Message);
			}
		}

		public static string GetUserMention(this GameResult result)
			=> CreateUserMention(result.UserName, result.UserId);

		public static string GetUserMention(this Player player)
			=> CreateUserMention(player.UserName, player.UserId);

		private static string CreateUserMention(string userName, int userId)
			=> @$"[{userName}](tg://user?id={userId})";
	}
}
