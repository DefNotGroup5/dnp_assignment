using Domain.DTOs;
using Domain.Models;

namespace Application.DaoInterfaces;

public interface IUserDao
{
    Task<User> CreateAsync(User user);
    Task<User?> GetByUsernameAsync(string username);
    Task<User?> GetByIdAsync(int id);
    Task Login(User user);
    Task Logout(User user);
}