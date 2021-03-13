using System.Collections.Generic;
using Telegram.Bot.Types.Enums;

namespace WfpBotConsole.Models
{
	public class Chat
	{
		public int Id { get; set; }

		public long ChatId { get; set; }

		public ChatType Type { get; set; }

		public string Title { get; set; }

		public ICollection<User> Users { get; set; }

		public bool IsArchived { get; set; }
	}
}
