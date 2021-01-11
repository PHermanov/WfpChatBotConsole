using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using WfpBotConsole.DB;
using WfpBotConsole.Commands;
using FluentScheduler;
using WfpBotConsole.Jobs;
using System.IO;

namespace WfpBotConsole
{
    class Program
    {
        private static ITelegramBotClient client;
        private static GameRepository repository;
        private static GameContext context;

        static void Main(string[] args)
        {
            ConfigureServices();
            SetUpJobs();

            var me = client.GetMeAsync().Result;
            Console.WriteLine($"Bot started {me.Id} : {me.FirstName}");

            client.StartReceiving();

            Console.WriteLine("Type exit to exit");

            while (true)
            {
                var input = Console.ReadLine();

                if (input == "exit")
                {
                    client.StopReceiving();
                    Console.WriteLine("Bot stopped");

                    break;
                }
            }
        }

        private static void ConfigureServices()
        {
            context = new GameContext();
            repository = new GameRepository(context);

            client = new TelegramBotClient(File.ReadAllText("key.secret"));

            client.OnMessage += Bot_OnMessage;
            client.OnReceiveError += Client_OnReceiveError;
            client.OnReceiveGeneralError += Client_OnReceiveGeneralError;
        }

        private static void SetUpJobs()
        {
            var getWinnerJob = new GetWinnerEverydayJob(repository, client);
            JobManager.AddJob(getWinnerJob, s => s.ToRunEvery(1).Days().At(12, 00));

            JobManager.Initialize();
        }

        private static void Client_OnReceiveGeneralError(object sender, ReceiveGeneralErrorEventArgs e)
        {
            Console.WriteLine(e.Exception.Message);
        }

        private static void Client_OnReceiveError(object sender, ReceiveErrorEventArgs e)
        {
            Console.WriteLine(e.ApiRequestException.Message);
        }

        static async void Bot_OnMessage(object sender, MessageEventArgs e)
        {
            if (!e.Message.From.IsBot && e.Message.Type == MessageType.Text && !string.IsNullOrWhiteSpace(e.Message.Text))
            {
                var chatId = e.Message.Chat.Id;
                var name = e.Message.From.Username;
                var text = e.Message.Text;

                var userName = e.Message.From.Username;

                if (string.IsNullOrWhiteSpace(userName))
                {
                    userName = e.Message.From.FirstName + " " + e.Message.From.LastName;
                }

                var newPlayer = await repository.CheckPlayer(chatId, e.Message.From.Id, userName);

                if (newPlayer)
                {
                    // await client.SendTextMessageAsync(chatId, string.Format(Messages.NewPlayerAdded, name));
                    Console.WriteLine(string.Format(Messages.NewPlayerAdded, name));
                }

                if (text.StartsWith(@"/"))
                {
                    Console.WriteLine($"Received a command in chat {chatId}. {name} : {text}");

                    await Command.Parse(text).Execute(e.Message.Chat.Id, client, repository);
                }
            }
        }
    }
}
