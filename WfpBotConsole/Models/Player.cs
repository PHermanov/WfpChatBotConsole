namespace WfpBotConsole.Models
{
    public class Player
    {
        public int Id { get; set; }
        public long ChatId { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }

        public override string ToString()
        {
            return $"{UserId}, {UserName}";
        }
    }
}
