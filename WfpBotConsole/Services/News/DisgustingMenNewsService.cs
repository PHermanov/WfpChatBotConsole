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
		private const string TestyiURL = "https://disgustingmen.com/tag/testyi/";

		private DateTime LastDichNewsDate { get; set; } = DateTime.UtcNow;
		private DateTime LastTestyiDate { get; set; } = DateTime.UtcNow;

		public async Task<IEnumerable<string>> GetNewsAsync(CancellationToken cancellationToken = default)
		{
			try
			{
				var news = new List<string>();
				news.AddRange(await GetDichNews(cancellationToken));
				news.AddRange(await GetTestyi(cancellationToken));

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
			var news = await GetPosts(DichURL, LastDichNewsDate, cancellationToken);

			if (news.Any())
			{
				LastDichNewsDate = news.First().EntryDate;
			}

			return news.Select(n => $"{Messages.NewDich}{Environment.NewLine}{n.Url}");
		}

		private async Task<IEnumerable<string>> GetTestyi(CancellationToken cancellationToken)
		{
			var tests = await GetPosts(TestyiURL, LastTestyiDate, cancellationToken);

			if (tests.Any())
			{
				LastTestyiDate = tests.First().EntryDate;
			}

			return tests.Select(n => $"{Messages.NewTest}{Environment.NewLine}{n.Url}");
		}

		private async Task<IEnumerable<PostModel>> GetPosts(
			string url,
			DateTime entryDate,
			CancellationToken cancellationToken)
		{
			var html = await url.GetStringAsync(cancellationToken);

			var doc = new HtmlDocument();
			doc.LoadHtml(html);

			return doc
				.DocumentNode
				.SelectNodes("//article")
				.Select(n => new PostModel
				{
					EntryDate = DateTime.Parse(n.SelectSingleNode("div[contains(@class, 'post-info')]/div[contains(@class, 'info')]/time[contains(@class, 'entry-date')]").GetAttributeValue("datetime", DateTime.UtcNow.ToString())),
					Url = n.SelectSingleNode("header[contains(@class, 'entry-header')]/h2[contains(@class, 'entry-title')]/a").GetAttributeValue("href", string.Empty)
				})
				.Where(n => !string.IsNullOrEmpty(n.Url) && n.EntryDate > entryDate)
				.OrderByDescending(n => n.EntryDate)
				.ToList();
		}
	}
}
