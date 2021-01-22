using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
{
	public class GameRepository
	{
		private readonly GameContext _context;

		public GameRepository(GameContext context)
		{
			_context = context;
		}

		public async Task<bool> CheckPlayer(long chatId, int userId, string userName)
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

			await _context.Players.AddAsync(newPlayer);

			await _context.SaveChangesAsync();

			return true;
		}

		public async Task<List<Player>> GetAllPlayersAsync(long chatId)
			=> await _context.Players.Where(p => p.ChatId == chatId).ToListAsync();

		public async Task<Player> GetPlayerByUserIdAsync(long chatId, int userId)
			=> await _context.Players.FirstOrDefaultAsync(p => p.ChatId == chatId && p.UserId == userId);

		public async Task<Player> GetPlayerByUserNameAsync(long chatId, string userName)
			=> await _context.Players.FirstOrDefaultAsync(p => p.ChatId == chatId && p.UserName == userName);

		public async Task<GameResult> GetTodayResultAsync(long chatId)
			=> await _context.Results.FirstOrDefaultAsync(r => r.ChatId == chatId && r.PlayedAt.Date == DateTime.Today);

		public async Task<GameResult> GetYesterdayResultAsync(long chatId)
			=> await _context.Results.FirstOrDefaultAsync(r => r.ChatId == chatId && r.PlayedAt.Date == DateTime.Today.AddDays(-1));

		public async Task<GameResult> GetLastPlayedGame(long chatId)
			=> await _context.Results
				.Where(r => r.ChatId == chatId)
				.OrderByDescending(r => r.PlayedAt)
				.FirstOrDefaultAsync();

		public async Task SaveGameResult(GameResult result)
		{
			await _context.Results.AddAsync(result);
			await _context.SaveChangesAsync();
		}

		public async Task<List<PlayerCountViewModel>> GetTopWinnersForMonth(long chatId, int top, DateTime date)
			=> await GetMonthResults(chatId, date).Take(top).ToListAsync();

		public async Task<PlayerCountViewModel> GetWinnerForMonth(long chatId, DateTime date)
			=> await GetMonthResults(chatId, date).FirstOrDefaultAsync();

		public async Task<long[]> GetAllChatsIds()
		 => await _context.Players.Select(p => p.ChatId).Distinct().ToArrayAsync();

		private IOrderedQueryable<PlayerCountViewModel> GetMonthResults(long chatId, DateTime date)
			=> _context.Results
				.Where(r => r.ChatId == chatId && r.PlayedAt.Date.Year == date.Year && r.PlayedAt.Date.Month == date.Month)
				.GroupBy(g => new
				{
					g.UserName,
					g.UserId
				})
				.Select(gr => new PlayerCountViewModel
				{
					UserId = gr.Key.UserId,
					UserName = gr.Key.UserName,
					Count = gr.Count()
				})
				.OrderByDescending(c => c.Count);

	}
}
