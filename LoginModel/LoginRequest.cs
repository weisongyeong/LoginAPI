namespace LoginModel;

public class LoginRequest
{
    public string Username { get; set; }
    public string Ip { get; set; }
    public string UserAgent { get; set; }
    public bool Succeeded { get; set; }
}
