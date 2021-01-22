﻿using System.Linq;
using WfpBotConsole.Models;

namespace WfpBotConsole.DB
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
					Count = gr.Count()
				})
				.OrderByDescending(c => c.Count);
	}
}
