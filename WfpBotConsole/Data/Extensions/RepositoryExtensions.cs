using System.Linq;
using WfpBotConsole.Models;

namespace WfpBotConsole.Data.Extensions
{
	public static class RepositoryExtensions
	{
		public static IOrderedQueryable<PlayerCountViewModel> ApplyGroupping(this IQueryable<GameResult> source)
			=> source.GroupBy(g => new
				{
					g.UserName,
					g.UserId
				})
				.Select(gr => new PlayerCountViewModel
				{
					UserId = gr.Key.UserId,
					UserName = gr.Key.UserName,
					Count = gr.Count(),
					LastWin = gr.Max(r => r.PlayedAt)
				})
				.OrderByDescending(c => c.Count)
				.ThenBy(c => c.LastWin);
	}
}
