using Application.DaoInterfaces;
using Domain.Models;

namespace FileData.DAOs;

public class UserFileDao : IUserDao
{
    private readonly FileContext context;

    public UserFileDao(FileContext context)
    {
        this.context = context;
    }
    
    public Task<User> CreateAsync(User user)
    {
        int userId = 1;
        if (context.Users != null && context.Users.Any())
        {
            userId = context.Users.Max(u => u.Id);
            userId++;
        }

        user.Id = userId;
        
        context.Users?.Add(user);
        context.SaveChanges();

        return Task.FromResult(user);
    }

    public Task<User?> GetByUsernameAsync(string username)
    {
        User? existing =
            context.Users?.FirstOrDefault(u => u.Username.Equals(username, StringComparison.OrdinalIgnoreCase));
        return Task.FromResult(existing);
    }

    public Task<User?> GetByIdAsync(int id)
    {
        User? user = context.Users?.FirstOrDefault(u => u.Id == id);
        return Task.FromResult(user);
    }

    public Task Login(User user)
    {
        User userToUpdate = user;
        userToUpdate.IsLoggedIn = true;
        context.Users?.Remove(user);
        context.Users?.Add(userToUpdate);
        context.SaveChanges();
        return Task.CompletedTask;
    }

    public Task Logout(User user)
    {
        User userToUpdate = user;
        userToUpdate.IsLoggedIn = false;
        context.Users?.Remove(user);
        context.Users?.Add(userToUpdate);
        context.SaveChanges();
        return Task.CompletedTask;
    }
}