namespace WfpBotConsole.Models
{
    public class PlayerCountViewModel
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"_{UserName}_: *{Count}*";
        }
    }
}
