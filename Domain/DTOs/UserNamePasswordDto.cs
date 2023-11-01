namespace Domain.DTOs;

public class UserNamePasswordDto
{
    public string Username { get; }
    public string Password { get; }

    public UserNamePasswordDto(string username, string password)
    {
        Username = username;
        Password = password;
    }
}