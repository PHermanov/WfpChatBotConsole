using WfpBotConsole.Models;

namespace WfpBotConsole
{
    public static class Extensions
    {
        public static string GetUserMention(this GameResult result)
            => @$"[{result.UserName}](tg://user?id={result.UserId})";
    }
}
