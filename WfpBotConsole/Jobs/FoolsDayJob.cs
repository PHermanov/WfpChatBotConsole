using FluentScheduler;
using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using Telegram.Bot.Types.InputFiles;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.DB;

namespace WfpBotConsole.Jobs
{
    [Inject]
    public class FoolsDayJob : IScheduleJob
    {
        private readonly IGameRepository _repository;
        private readonly ITelegramBotClient _client;

        public FoolsDayJob(IGameRepository repository, ITelegramBotClient client)
        {
            _repository = repository;
            _client = client;
        }

        public async void Execute()
        {
            if (DateTime.Today.Month != 4)
            {
                return;
            }

            var allChatIds = await _repository.GetAllChatsIdsAsync();

            await Execute(allChatIds);
        }

        public void Schedule()
        {
            JobManager.AddJob(this, s => s.WithName(nameof(FoolsDayJob)).ToRunEvery(0).Months().On(1).At(9, 30));
        }

        public async Task Execute(params long[] chatIds)
        {
            for (long i = 0; i < chatIds.Length; i++)
            {
                try
                {
                    // using var image = Image.FromFile("Images/april.png");
                    await _client.TrySendPhotoAsync(chatIds[i], new InputOnlineFile("Images/april.png"), "message", ParseMode.Markdown);
                }
                catch (Exception e)
                {
                    Console.WriteLine(@"Exception when executing FoolsDayJob: " + e.Message);
                }
            }
        }
    }
}
