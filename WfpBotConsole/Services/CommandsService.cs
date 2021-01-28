﻿using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WfpBotConsole.Commands;
using WfpBotConsole.Core.Attributes;
using WfpBotConsole.Core.Enums;

namespace WfpBotConsole.Services
{
	[Inject(RegistrationScope.Singleton)]
	public class CommandsService : ICommandsService
	{
		private readonly IEnumerable<ICommand> _commands;

		public CommandsService(IEnumerable<ICommand> commands)
		{
			_commands = commands;
		}

		public async Task Execute(long chatId, string commandKey)
		{
			var commandKeyLowered = commandKey.ToLower();

			var command = _commands.FirstOrDefault(c => c.CommandKey == commandKeyLowered);

			if (command == null)
			{
				command = _commands.FirstOrDefault(c => c.GetType() == typeof(HelpCommand));
			}

			await command.Execute(chatId);
		}
	}
}
