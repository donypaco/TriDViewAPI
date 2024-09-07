namespace TaskManagementSystem.Data
{
    public class Log
    {
        public int Id { get; set; }
        public DateTimeOffset Timestamp { get; set; }
        public string Level { get; set; }
        public string Message { get; set; }
        public string Exception { get; set; }
    }
}
