using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	[Inject]
	public class GameRepository : IGameRepository
	{
		private readonly IGameContext _gameContext;

		public GameRepository(IGameContext context)
		{
			_gameContext = context;
		}

		public async Task<bool> CheckPlayerAsync(long chatId, int userId, string userName)
		{
			var playerById = await GetPlayerByUserIdAsync(chatId, userId);

			if (playerById != null)
			{
				return false;
			}

			var newPlayer = new Player()
			{
				ChatId = chatId,
				UserId = userId,
				UserName = userName
			};

			await _gameContext.Players.AddAsync(newPlayer);

			await _gameContext.SaveChangesAsync();

			return true;
		}

		public async Task<List<Player>> GetAllPlayersAsync(long chatId)
			=> await _gameContext.Players.Where(p => p.ChatId == chatId).ToListAsync();

		public async Task<Player> GetPlayerByUserIdAsync(long chatId, int userId)
			=> await _gameContext.Players.FirstOrDefaultAsync(p => p.ChatId == chatId && p.UserId == userId);

		public async Task<Player> GetPlayerByUserNameAsync(long chatId, string userName)
			=> await _gameContext.Players.FirstOrDefaultAsync(p => p.ChatId == chatId && p.UserName == userName);

		public async Task<GameResult> GetTodayResultAsync(long chatId)
			=> await _gameContext.Results.FirstOrDefaultAsync(r => r.ChatId == chatId && r.PlayedAt.Date == DateTime.Today);

		public async Task<GameResult> GetYesterdayResultAsync(long chatId)
			=> await _gameContext.Results.FirstOrDefaultAsync(r => r.ChatId == chatId && r.PlayedAt.Date == DateTime.Today.AddDays(-1));

		public async Task<GameResult> GetLastPlayedGameAsync(long chatId)
			=> await _gameContext.Results
				.Where(r => r.ChatId == chatId)
				.OrderByDescending(r => r.PlayedAt)
				.FirstOrDefaultAsync();

		public async Task SaveGameResultAsync(GameResult result)
		{
			await _gameContext.Results.AddAsync(result);
			await _gameContext.SaveChangesAsync();
		}

		public async Task<List<PlayerCountViewModel>> GetTopWinnersForMonthAsync(long chatId, int top, DateTime date)
			=> await GetMonthResults(chatId, date).Take(top).ToListAsync();

		public async Task<PlayerCountViewModel> GetWinnerForMonthAsync(long chatId, DateTime date)
			=> await GetMonthResults(chatId, date).FirstOrDefaultAsync();

		public async Task<long[]> GetAllChatsIdsAsync()
			=> await _gameContext.Players.Select(p => p.ChatId).Distinct().ToArrayAsync();

		public async Task<List<PlayerCountViewModel>> GetAllWinnersAsync(long chatId)
			=> await _gameContext.Results.Where(r => r.ChatId == chatId).ApplyGroupping().ToListAsync();

		private IOrderedQueryable<PlayerCountViewModel> GetMonthResults(long chatId, DateTime date)
			=> _gameContext.Results
				.Where(r => r.ChatId == chatId && r.PlayedAt.Date.Year == date.Year && r.PlayedAt.Date.Month == date.Month)
				.ApplyGroupping();

	}
}
