using Flurl.Http;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Services.News
{
	[Inject(RegistrationScope.Singleton)]
	public class DisgustingMenNewsService : INewsService
	{
		private const string DichURL = "https://disgustingmen.com/tag/dich/";

		private DateTime LastDichNewsDate { get; set; } = DateTime.UtcNow;

		public async Task<IEnumerable<string>> GetNewsAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var news = new List<string>();
				news.AddRange(await GetDichNews(cancellationToken));

				return news;
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.GetType());
				Console.WriteLine(ex.Message);
			}

			return Enumerable.Empty<string>();
		}

		private async Task<IEnumerable<string>> GetDichNews(CancellationToken cancellationToken)
		{
			var html = await DichURL.GetStringAsync(cancellationToken);

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			var news = doc
				.DocumentNode
				.SelectNodes("//article")
				.Select(n => new
				{
					EntryDate = DateTime.Parse(n.SelectSingleNode("div[contains(@class, 'post-info')]/div[contains(@class, 'info')]/time[contains(@class, 'entry-date')]").GetAttributeValue("datetime", DateTime.UtcNow.ToString())),
					Url = n.SelectSingleNode("header[contains(@class, 'entry-header')]/h2[contains(@class, 'entry-title')]/a").GetAttributeValue("href", string.Empty)
				})
				.Where(n => !string.IsNullOrEmpty(n.Url) && n.EntryDate > LastDichNewsDate)
				.OrderByDescending(n => n.EntryDate)
				.ToList();

			if (news.Any())
			{
				LastDichNewsDate = news.First().EntryDate;
			}

			return news.Select(n => $"{Messages.NewDich}{Environment.NewLine}{n.Url}");
		}
	}
}
