namespace Net6Chat.Domain.Models
{
    public class Message
    {
        public Guid Id { get; set; }
        public string? Room { get; set; }
        public string? UserName { get; set; }
        public string? Text { get; set; }
        public DateTime Created { get; set; }
    }
}
