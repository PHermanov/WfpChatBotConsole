using FluentScheduler;
using System.Threading.Tasks;
using Telegram.Bot;
using WfpBotConsole.Commands;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
	public class GetWinnerEverydayJob : IScheduleJob
	{
		private readonly IGameRepository _repository;
		private readonly ITelegramBotClient _client;

		public GetWinnerEverydayJob(IGameRepository repository, ITelegramBotClient client)
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
			var checkMissedGamesCommand = new CheckMissedGamesCommand();
			var newWinnerCommand = new NewWinnerCommand();

			for (int i = 0; i < chatIds.Length; i++)
			{
				await checkMissedGamesCommand.Execute(chatIds[i], _client, _repository);
				await newWinnerCommand.Execute(chatIds[i], _client, _repository);
			}
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Days().At(12, 00));
		}
	}
}
