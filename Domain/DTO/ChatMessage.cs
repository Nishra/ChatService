using Domain.Enum;

namespace Domain.DTO
{
    public class ChatMessage
    {
        public Guid MessageId { get; set; }
        public string UserName { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public DateTime InitiatedDate { get; set; }
        public Status Status { get; set; }
    }
}
