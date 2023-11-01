using System.Security.Claims;
using Domain.DTOs;
using Domain.Models;

namespace HttpClients.ClientInterfaces;

public interface IUserService
{
    public Task<User> CreateAsync(UserNamePasswordDto dto);
    public Task Login(UserNamePasswordDto dto);
    public Task Logout();
    public Task<ClaimsPrincipal> GetAuthAsync();

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; }
}