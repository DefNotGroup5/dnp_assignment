namespace Domain.DTOs;

public class UserNameDto
{
    public string Username { get; }

    public UserNameDto(string username)
    {
        Username = username;
    }
}