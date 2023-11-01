using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class PostLogic : IPostLogic
{
    private readonly IPostDao postDao;
    private readonly IUserDao userDao;

    public PostLogic(IPostDao postDao, IUserDao userDao)
    {
        this.postDao = postDao;
        this.userDao = userDao;
    }

    public async Task<Post> CreateAsync(PostCreationDto dto)
    {
        User? user = await userDao.GetByUsernameAsync(dto.Username);
        if (user == null)
            throw new Exception($"User with Name {dto.Username} does not exist!");
        if (!user.IsLoggedIn)
            throw new Exception("User must be logged in to create a post!");
        ValidatePostDto(dto);
        Post post = new Post(dto.Title, dto.Body, user);
        Post created = await postDao.CreateAsync(post);
        return created;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        IEnumerable<Post> posts = await postDao.GetAsync();
        return posts;
    }

    public async Task<Post> GetByIdAsync(int id)
    {
        Post? post = await postDao.GetByIdAsync(id);
        if (post == null)
            throw new Exception($"Post with ID {id} does not exist");
        return post;
    }

    private void ValidatePostDto(PostCreationDto dto)
    {
        if (string.IsNullOrEmpty(dto.Title)) throw new Exception("Title cannot be empty!");
        if (string.IsNullOrEmpty(dto.Body)) throw new Exception("Body cannot be empty!");
    }
}