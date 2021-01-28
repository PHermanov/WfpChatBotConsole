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
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;
using WfpBotConsole.Resources;

namespace WfpBotConsole.Jobs
{
	[Inject]
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

		public async Task Execute(params long[] chatIds)
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

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Months().OnTheLastDay().At(12, 05));
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

				using var avatarImage = Image.FromStream(avatarStream);
				using var bitmap = new Bitmap(avatarImage.Width, avatarImage.Height);
				using var canvas = Graphics.FromImage(bitmap);

				canvas.InterpolationMode = InterpolationMode.HighQualityBicubic;
				canvas.SmoothingMode = SmoothingMode.AntiAlias;
				canvas.PixelOffsetMode = PixelOffsetMode.HighQuality;

				canvas.DrawImage(avatarImage, new Point());

				var hPadding = bitmap.Height / 10;
				var vPadding = hPadding / 2;

				// add bowl image to avatar
				var bowlRatio = (double)(bitmap.Height / 2.5) / bowlImage.Height;
				var bowlWidth = (int)(bowlImage.Width * bowlRatio);
				var bowlHeight = (int)(bowlImage.Height * bowlRatio);

				canvas.DrawImage(
					bowlImage,
					new Rectangle(hPadding, bitmap.Height - bowlHeight - vPadding, bowlWidth, bowlHeight),
					new Rectangle(0, 0, bowlImage.Width, bowlImage.Height),
					GraphicsUnit.Pixel);

				// add month and year text to avatar
				using var gp = new GraphicsPath();
				var stringFormat = new StringFormat
				{
					Alignment = StringAlignment.Center,
					LineAlignment = StringAlignment.Center
				};

				float fontSize = 60;
				using var font = new Font("Impact", fontSize, FontStyle.Bold, GraphicsUnit.Pixel);

				gp.AddString(
					DateTime.Today.ToString($"MMMM{Environment.NewLine}yyyy", new System.Globalization.CultureInfo("en-US")),
					font.FontFamily,
					(int)font.Style,
					fontSize,
					new Rectangle(hPadding + bowlWidth, bitmap.Height - bowlHeight - vPadding, bitmap.Width - bowlWidth - hPadding, bowlHeight),
					stringFormat);

				canvas.DrawPath(new Pen(Color.Black, 8) { LineJoin = LineJoin.Round }, gp);
				canvas.FillPath(Brushes.White, gp);

				canvas.Save();
				bitmap.Save(winnerImageStream, ImageFormat.Png);
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
