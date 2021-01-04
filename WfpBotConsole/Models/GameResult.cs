using System;

namespace WfpBotConsole.Models
{
    public class GameResult
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public DateTime PlayedAt { get; set; }
    }
}
