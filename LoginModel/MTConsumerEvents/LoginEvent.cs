namespace LoginModel.MTConsumerEvents;

public class LoginEvent
{
    public string CorrelationId { get; set; }
    public string Username { get; set; }
    public string Ip { get; set; }
    public string UserAgent { get; set; }
    public bool Succeeded { get; set; }
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}