using FluentScheduler;
using System;
using Telegram.Bot;
using WfpBotConsole.DB;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Jobs
{
	public class WednesdayJob : IScheduleJob
	{
		private readonly IGameRepository _repository;
		private readonly ITelegramBotClient _client;

		public WednesdayJob(IGameRepository repository, ITelegramBotClient client)
		{
			_repository = repository;
			_client = client;
		}

		public async void Execute()
		{
			var allChatIds = await _repository.GetAllChatsIdsAsync();

			for (int i = 0; i < allChatIds.Length; i++)
			{
				await _client.TrySendTextMessageAsync(allChatIds[i], Messages.WednesdayMyDudes);
				await _client.TrySendStickerAsync(allChatIds[i], StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Frog));
			}
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday).At(11, 00));
		}
	}
}
