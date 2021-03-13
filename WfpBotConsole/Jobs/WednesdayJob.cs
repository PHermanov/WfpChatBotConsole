using FluentScheduler;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Data;
using WfpBotConsole.Resources;
using WfpBotConsole.Stickers;

namespace WfpBotConsole.Jobs
{
	[Inject]
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

			await Execute(allChatIds);
		}

		public async Task Execute(params long[] chatIds)
		{
			for (int i = 0; i < chatIds.Length; i++)
			{
				await _client.TrySendTextMessageAsync(chatIds[i], Messages.WednesdayMyDudes);
				await _client.TrySendStickerAsync(chatIds[i], StickersSelector.SelectRandomFromSet(StickersSelector.SticketSet.Frog));
			}
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.WithName(nameof(WednesdayJob)).ToRunEvery(0).Weeks().On(DayOfWeek.Wednesday).At(11, 00));
		}
	}
}
