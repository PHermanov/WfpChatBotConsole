﻿using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Commands
{
	public class TestCommand : Command
	{
		public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository = null)
		{
			await client.SendTextMessageAsync(chatId, $"Хуест!", parseMode: ParseMode.Markdown);
			await client.SendStickerAsync(chatId, StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Yoba));
		}
	}
}
