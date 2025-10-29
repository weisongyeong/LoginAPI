using System.ComponentModel.DataAnnotations;

namespace LoginData.Models;

public class LoginLog
{
    [Key]
    public Guid Id { get; set; }

    [Required]
    public string CorrelationId { get; set; }

    public string Username { get; set; }
    public string Ip { get; set; }
    public string UserAgent { get; set; }
    public bool Succeeded { get; set; }
    public DateTime Timestamp { get; set; }
}

