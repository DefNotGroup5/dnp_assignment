using System.ComponentModel.DataAnnotations;

namespace Domain.Models;

public class Post
{
    [Key]
    public int Id { get; set; }
    public string Title { get; private set; }
    public string Body { get; private set; }
    public User Owner { get; set; }
    public int OwnerId { get; set; }

    public Post(string title, string body, int ownerId)
    {
        Title = title;
        Body = body;
        OwnerId = ownerId;
    }
    
    private Post() {}
}