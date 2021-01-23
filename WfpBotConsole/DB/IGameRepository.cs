using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	public interface IGameRepository
	{
		Task<bool> CheckPlayerAsync(long chatId, int userId, string userName);
		Task<long[]> GetAllChatsIdsAsync();
		Task<List<Player>> GetAllPlayersAsync(long chatId);
		Task<List<PlayerCountViewModel>> GetAllWinnersAsync(long chatId);
		Task<GameResult> GetLastPlayedGameAsync(long chatId);
		Task<Player> GetPlayerByUserIdAsync(long chatId, int userId);
		Task<Player> GetPlayerByUserNameAsync(long chatId, string userName);
		Task<GameResult> GetTodayResultAsync(long chatId);
		Task<List<PlayerCountViewModel>> GetTopWinnersForMonthAsync(long chatId, int top, DateTime date);
		Task<PlayerCountViewModel> GetWinnerForMonthAsync(long chatId, DateTime date);
		Task<GameResult> GetYesterdayResultAsync(long chatId);
		Task SaveGameResultAsync(GameResult result);
	}
}