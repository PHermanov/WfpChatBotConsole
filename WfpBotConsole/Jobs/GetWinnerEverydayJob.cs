using FluentScheduler;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WfpBotConsole.Commands;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
	[Inject]
	public class GetWinnerEverydayJob : IScheduleJob
	{
		private readonly IGameRepository _gameRepository;

		private readonly ICommand _checkMissedGamesCommand;
		private readonly ICommand _newWinnerCommand;

		public GetWinnerEverydayJob(
			IEnumerable<ICommand> commands,
			IGameRepository gameRepository)
		{
			_gameRepository = gameRepository;

			_checkMissedGamesCommand = commands.FirstOrDefault(c => c.GetType() == typeof(CheckMissedGamesCommand));
			_newWinnerCommand = commands.FirstOrDefault(c => c.GetType() == typeof(NewWinnerCommand));
		}

		public async void Execute()
		{
			var allChatIds = await _gameRepository.GetAllChatsIdsAsync();

			await Execute(allChatIds);
		}

		public async Task Execute(params long[] chatIds)
		{
			for (int i = 0; i < chatIds.Length; i++)
			{
				await _checkMissedGamesCommand.Execute(chatIds[i]);
				await _newWinnerCommand.Execute(chatIds[i]);
			}
		}

		public void Schedule()
		{
			JobManager.AddJob(this, s => s.ToRunEvery(0).Days().At(12, 00));
		}
	}
}
