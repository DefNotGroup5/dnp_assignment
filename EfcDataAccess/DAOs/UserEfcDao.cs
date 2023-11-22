using Application.DaoInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;

public class UserEfcDao : IUserDao
{
    private readonly PostContext postContext;

    public UserEfcDao(PostContext postContext)
    {
        this.postContext = postContext;
    }

    public async Task<User> CreateAsync(User user)
    {
        EntityEntry<User> newUser = await postContext.Users.AddAsync(user);
        await postContext.SaveChangesAsync();
        return newUser.Entity;
    }

    public async Task<User?> GetByUsernameAsync(string username)
    {
        User? existing = await postContext.Users.FirstOrDefaultAsync(u =>
            u.Username.ToLower().Equals(username.ToLower()));
        return existing;
    }

    public async Task<User?> GetByIdAsync(int id)
    {
        User? user = await postContext.Users.FindAsync(id);
        return user;
    }

    public async Task Login(User user)
    {
        User newUser = user;
        newUser.IsLoggedIn = true;
        postContext.Users.Update(user);
        await postContext.SaveChangesAsync();
    }

    public async Task Logout(User user)
    {
        User newUser = user;
        newUser.IsLoggedIn = false;
        postContext.Users.Update(user);
        await postContext.SaveChangesAsync();
    }
}