using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Models;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Commands
{
    public class NewWinnerCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository = null)
        {
            var todayResult = await repository.GetTodayResultAsync(chatId);

            var messageTemplate = Messages.TodayWinnerAlreadySet;

            if (todayResult == null)
            {
                var users = await repository.GetAllPlayersAsync(chatId);

                var newWinner = users[new Random().Next(users.Count)];

                messageTemplate = Messages.NewWinner;

                todayResult = new GameResult()
                {
                    ChatId = chatId,
                    UserId = newWinner.UserId,
                    UserName = newWinner.UserName,
                    PlayedAt = DateTime.Today
                };

                await repository.SaveGameResultAsync(todayResult);
            }

            var msg = string.Format(messageTemplate, todayResult.GetUserMention());

            await client.TrySendTextMessageAsync(chatId, msg, ParseMode.Markdown);
            await client.TrySendStickerAsync(chatId, StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Yoba));
        }
    }
}
