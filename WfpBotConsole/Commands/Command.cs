﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Types;
using WfpBotConsole.DB;

namespace WfpBotConsole.Commands
{
    public abstract class Command
    {
        private static readonly Dictionary<string, Command> _commands = new()
        {
            { @"/help", new HelpCommand() },
            { @"/test", new TestCommand() },
            { @"/showusers", new ShowUsersCommand() },
            { @"/today", new WinnerTodayCommand() },
            { @"/month", new CurrentMonthTopWinnersCommand() }
        };

        public abstract Task Execute(Message message, ITelegramBotClient client, GameRepository repository = null);

        public static Command Parse(string name)
        {
            var nameLowered = name.ToLower();
            return _commands.ContainsKey(nameLowered) ? _commands[nameLowered] : new HelpCommand();
        }
    }
}
