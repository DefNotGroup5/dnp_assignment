using Application.DaoInterfaces;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;

namespace Application.Logic;

public class UserLogic : IUserLogic
{
    private readonly IUserDao userDao;

    public UserLogic(IUserDao userDao)
    {
        this.userDao = userDao;
    }
    
    public async Task<User> CreateAsync(UserNamePasswordDto dto)
    {
        User? existing = await userDao.GetByUsernameAsync(dto.Username);
        if (existing != null)
            throw new Exception("Username already taken!");
        
        ValidateData(dto);
        User toCreate = new User(dto.Username, dto.Password);

        User created = await userDao.CreateAsync(toCreate);

        return created;
    }

    public async Task Login(UserNamePasswordDto dto)
    {
        User? existing = await userDao.GetByUsernameAsync(dto.Username);
        if (existing == null)
            throw new Exception("The username or password is incorrect!");
        if (!existing.Password.Equals(dto.Password))
            throw new Exception("The username or password is incorrect!");
        await userDao.Login(existing);
    }

    public async Task Logout(UserNameDto dto)
    {
        User? existing = await userDao.GetByUsernameAsync(dto.Username);
        if (existing == null)
            throw new Exception("A user with this username does not exist!");
        await userDao.Logout(existing);
    }

    public async Task<User> GetUserByUsernameAsync(string username)
    {
        User? user = await userDao.GetByUsernameAsync(username);
        if (user == null)
            throw new Exception("User does not exist lol");
        return user;
    }

    private static void ValidateData(UserNamePasswordDto userToCreate)
    {
        string username = userToCreate.Username;
        string password = userToCreate.Password;
        
        if (username.Length < 2)
            throw new Exception("Username must be at least 2 characters!");

        if (username.Length > 15)
            throw new Exception("Username must be less than 15 characters!");

        if (password.Length < 7)
            throw new Exception("Password must contain at least 7 characters!");
        if (password.Length > 20)
            throw new Exception("Password must be less than 20 characters!");
    }
}