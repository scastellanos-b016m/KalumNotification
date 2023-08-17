namespace KalumNotification.DTOs
{
    public class NotificationRequestDTO
    {
        public string Email { get; set ; }
        public string Recipient { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }
        public string IdentificationId { get; set; }
        public string Type { get; set; }
        
    }
}