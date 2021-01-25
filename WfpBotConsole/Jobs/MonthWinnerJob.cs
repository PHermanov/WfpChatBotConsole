using FluentScheduler;
using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
	public class MonthWinnerJob : IScheduleJob
	{
		private readonly IGameRepository _repository;
		private readonly ITelegramBotClient _client;

		public MonthWinnerJob(IGameRepository repository, ITelegramBotClient client)
		{
			_repository = repository;
			_client = client;
		}

		public async void Execute()
		{
			var allChatIds = await _repository.GetAllChatsIdsAsync();

			await Execute(allChatIds);
		}

		public async Task Execute(long chatId)
		{
			await Execute(chatId);
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Months().OnTheLastDay().At(12, 05));
		}

		private async Task Execute(params long[] chatIds)
		{
			for (int i = 0; i < chatIds.Length; i++)
			{
				var monthWinner = await _repository.GetWinnerForMonthAsync(chatIds[i], DateTime.Today);

				if (monthWinner != null)
				{
					var mention = monthWinner.GetUserMention();
					var message = $"{Messages.MonthWinner}{Environment.NewLine}\u269C {mention} \u269C{Environment.NewLine}{Messages.Congrats}";

					var userProfilePhotos = await _client.GetUserProfilePhotosAsync(monthWinner.UserId);
					using var winnerImage = await GetWinnerImage(userProfilePhotos);

					await _client.TrySendPhotoAsync(chatIds[i], new InputOnlineFile(winnerImage), message, ParseMode.Markdown);
				}
			}
		}

		private async Task<Stream> GetWinnerImage(UserProfilePhotos userProfilePhotos)
		{
			using var bowlImage = Image.FromFile("Images/bowl.png");

			var winnerImageStream = new MemoryStream();

			if (userProfilePhotos.Photos.Any())
			{
				var photoSize = userProfilePhotos.Photos[0]
					.OrderByDescending(p => p.Height)
					.FirstOrDefault();

				var photoFile = await _client.GetFileAsync(photoSize.FileId);

				using var avatarStream = new MemoryStream();

				await _client.DownloadFileAsync(photoFile.FilePath, avatarStream);

				using (var avatarImage = Image.FromStream(avatarStream))
				using (var bitmap = new Bitmap(avatarImage.Width, avatarImage.Height))
				using (var canvas = Graphics.FromImage(bitmap))
				{
					canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;

					canvas.DrawImage(avatarImage, new Point());

					int bowlSize = bitmap.Width / 2;

					canvas.DrawImage(
						bowlImage,
						new Rectangle(0, bitmap.Height - bowlSize, bowlSize, bowlSize),
						new Rectangle(0, 0, bowlImage.Width, bowlImage.Height),
						GraphicsUnit.Pixel);

					canvas.Save();
					bitmap.Save(winnerImageStream, ImageFormat.Png);
				}
			}
			else
			{
				bowlImage.Save(winnerImageStream, ImageFormat.Png);
			}

			winnerImageStream.Seek(0, SeekOrigin.Begin);

			return winnerImageStream;
		}
	}
}
