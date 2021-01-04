namespace WfpBotConsole.Models
{
    public class PlayerCountViewModel
    {
        public string Name { get; set; }
        public int Count { get; set; }

        public override string ToString()
        {
            return $"_{Name}_: *{Count}*";
        }
    }
}
