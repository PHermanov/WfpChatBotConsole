using FluentScheduler;
using Telegram.Bot;
using WfpBotConsole.Commands;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
    public class GetWinnerEverydayJob : IJob
    {
        private readonly GameRepository _repository;
        private readonly ITelegramBotClient _client;

        public GetWinnerEverydayJob(GameRepository repository, ITelegramBotClient client)
        {
            _repository = repository;
            _client = client;
        }

        public async void Execute()
        {
            var allChatIds = await _repository.GetAllChatsIds();

            var command = new NewWinnerCommand();

            for (int i = 0; i < allChatIds.Length; i++)
            {
                await command.Execute(allChatIds[i], _client, _repository); 
            }
        }
    }
}
