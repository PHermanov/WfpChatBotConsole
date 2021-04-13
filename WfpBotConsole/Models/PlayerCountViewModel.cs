using System;

namespace WfpBotConsole.Models
{
	public class PlayerCountViewModel
	{
		public int UserId { get; set; }
		public string UserName { get; set; }
		public int Count { get; set; }
		public DateTime LastWin { get; set; }

		public override string ToString()
		{
			return $"<i>{UserName}</i>: <b>{Count}</b>";
		}
	}
}
