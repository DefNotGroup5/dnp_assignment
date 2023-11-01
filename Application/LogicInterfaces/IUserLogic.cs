using Domain.DTOs;
using Domain.Models;

namespace Application.LogicInterfaces;

public interface IUserLogic
{
    Task<User> CreateAsync(UserNamePasswordDto userToCreate);
    Task Login(UserNamePasswordDto dto);
    Task Logout(UserNameDto dto);
    Task<User> GetUserByUsernameAsync(string username);
}