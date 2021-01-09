using WfpBotConsole.Models;

namespace WfpBotConsole
{
	public static class Extensions
	{
		public static string GetUserMention(this GameResult result)
			=> CreateUserMention(result.UserName, result.UserId);

		public static string GetUserMention(this Player player)
			=> CreateUserMention(player.UserName, player.UserId);

		private static string CreateUserMention(string userName, int userId)
			=> @$"[{userName}](tg://user?id={userId})";
	}
}
