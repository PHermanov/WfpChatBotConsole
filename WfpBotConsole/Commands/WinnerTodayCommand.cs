using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Models;

namespace WfpBotConsole.Commands
{
    public class WinnerTodayCommand : Command
    {
        public override async Task Execute(long chatId, ITelegramBotClient client, GameRepository repository)
        {
            var todayResult = await repository.GetTodayResultAsync(chatId);

            if (todayResult != null)
            {
                var msg = string.Format(Messages.TodayWinnerAlreadySet, todayResult.UserName);

                await client.SendTextMessageAsync(chatId, msg, ParseMode.Markdown);
            }
            else
            {
                var users = await repository.GetAllPlayersAsync(chatId);
                
                var newWinner = users[new Random().Next(users.Count)];

                var gameResult = new GameResult() 
                { 
                    ChatId = chatId,
                    UserId = newWinner.UserId,
                    UserName = newWinner.UserName,
                    PlayedAt = DateTime.Today
                };

                await repository.SaveGameResult(gameResult);

                var msg = string.Format(Messages.NewWinner, newWinner.UserName);

                await client.SendTextMessageAsync(chatId, msg, ParseMode.Markdown);
            }
        }
    }
}
