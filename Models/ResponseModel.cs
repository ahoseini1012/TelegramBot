namespace Bot
{
    public class ResponseModel
    {
        public long Id { get; set; }
        public long ChatId { get; set; }
        public long MessageId { get; set; }
        public string SqlQuery { get; set; }=String.Empty;
        public string Action { get; set; }=String.Empty;

    }
}