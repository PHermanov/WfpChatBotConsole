using System.Collections.Generic;

namespace WfpBotConsole.Models
{
	public class User
	{
		public int Id { get; set; }

		public int UserId { get; set; }

		public ICollection<Chat> Chats { get; set; }

		public bool IsBot { get; set; }

		public string FirstName { get; set; }

		public string LastName { get; set; }

		public string Username { get; set; }

		public string LanguageCode { get; set; }

		public bool IsArchived { get; set; }
	}
}
