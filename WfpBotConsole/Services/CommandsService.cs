using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WfpBotConsole.Commands;
using WfpBotConsole.Core.Attributes;

namespace WfpBotConsole.Services
{
	[Inject]
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

			if (command != null)
			{
				await command.Execute(chatId);
			}
		}
	}
}
