using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IPostService
{
    public Task<Post> CreateAsync(PostCreationDto dto);
    public Task<IEnumerable<Post>> GetPosts();
    public Task<Post> GetById(int id);
}