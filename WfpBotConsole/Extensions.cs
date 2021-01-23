using System;
using System.Collections.Generic;
using System.Linq;
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
		private static readonly Dictionary<char, string> digits = new()
		{
			{ '0', "\U00000030\U000020E3" },
			{ '1', "\U00000031\U000020E3" },
			{ '2', "\U00000032\U000020E3" },
			{ '3', "\U00000033\U000020E3" },
			{ '4', "\U00000034\U000020E3" },
			{ '5', "\U00000035\U000020E3" },
			{ '6', "\U00000036\U000020E3" },
			{ '7', "\U00000037\U000020E3" },
			{ '8', "\U00000038\U000020E3" },
			{ '9', "\U00000039\U000020E3" }
		};

		public static async Task TrySendTextMessageAsync(
			this ITelegramBotClient client,
			ChatId chatId,
			string text,
			ParseMode parseMode = ParseMode.Default,
			bool disableWebPagePreview = false,
			bool disableNotification = false,
			int replyToMessageId = 0,
			IReplyMarkup replyMarkup = null,
			CancellationToken cancellationToken = default)
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

		public static async Task TrySendStickerAsync(
			this ITelegramBotClient client,
			ChatId chatId,
			InputOnlineFile sticker,
			bool disableNotification = false,
			int replyToMessageId = 0,
			IReplyMarkup replyMarkup = null,
			CancellationToken cancellationToken = default)
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

		public static async Task TrySendPhotoAsync(
			this ITelegramBotClient client,
			ChatId chatId,
			InputOnlineFile photo,
			string caption = null,
			ParseMode parseMode = ParseMode.Default,
			bool disableNotification = false,
			int replyToMessageId = 0,
			IReplyMarkup replyMarkup = null,
			CancellationToken cancellationToken = default)
		{
			try
			{
				await client.SendPhotoAsync(chatId, photo, caption, parseMode, disableNotification, replyToMessageId, replyMarkup, cancellationToken);
			}
			catch (Exception exception)
			{
				Console.WriteLine(exception.GetType());
				Console.WriteLine(exception.Message);
			}
		}

		public static string ReplaceDigits(this string value)
		{
			return string.Join(string.Empty, value.ToCharArray().Select(c => digits.ContainsKey(c) ? digits[c] : c.ToString()));
		}

		public static string GetUserMention(this GameResult result)
			=> CreateUserMention(result.UserName, result.UserId);

		public static string GetUserMention(this Player player)
			=> CreateUserMention(player.UserName, player.UserId);

		public static string GetUserMention(this PlayerCountViewModel player)
			=> CreateUserMention(player.UserName, player.UserId);

		private static string CreateUserMention(string userName, int userId)
			=> @$"[{userName}](tg://user?id={userId})";
	}
}
