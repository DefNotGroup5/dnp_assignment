namespace Domain.DTOs;

public class PostCreationDto
{
    public string Username { get; set; }
    public string Title { get; }
    public string Body { get; }

    public PostCreationDto(string username, string title, string body)
    {
        Username = username;
        Title = title;
        Body = body;
    }
}