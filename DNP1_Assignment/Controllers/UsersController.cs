using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace DNP1_Assignment.Controllers;

[ApiController]
[Route("[controller]")]
public class UsersController : ControllerBase
{
    private readonly IUserLogic userLogic;
    private readonly IConfiguration config;

    public UsersController(IConfiguration config,IUserLogic userLogic)
    {
        this.userLogic = userLogic;
        this.config = config;
    }
    
    private List<Claim> GenerateClaims(User user)
    {
        var claims = new[]
        {
            new Claim(JwtRegisteredClaimNames.Sub, config["Jwt:Subject"]),
            new Claim(JwtRegisteredClaimNames.Iat, DateTime.UtcNow.ToString()),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            new Claim(ClaimTypes.Name, user.Username),
            new Claim("UserId",user.Id.ToString())
        };
        return claims.ToList();
    }

    private string GenerateJwt(User user)
    {
        List<Claim> claims = GenerateClaims(user);
        SymmetricSecurityKey key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(config["Jwt:Key"]));
        SigningCredentials signIn = new SigningCredentials(key, SecurityAlgorithms.HmacSha512);
        JwtHeader header = new JwtHeader(signIn);
        JwtPayload payload = new JwtPayload(
            config["Jwt:Issuer"],
            config["Jwt:Audience"],
            claims, 
            null,
            DateTime.UtcNow.AddMinutes(60));
        JwtSecurityToken token = new JwtSecurityToken(header, payload);
        string serializedToken = new JwtSecurityTokenHandler().WriteToken(token);
        return serializedToken;
    }
    
    [HttpPost]
    public async Task<ActionResult<User>>? CreateAsync(UserNamePasswordDto dto)
    {
        try
        {
            User user = await userLogic.CreateAsync(dto);
            return Created($"/users/{user.Id}", user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpPatch("Login")]
    public async Task<ActionResult> Login(UserNamePasswordDto dto)
    {
        try
        {
            await userLogic.Login(dto);
            User user = await userLogic.GetUserByUsernameAsync(dto.Username);
            string token = GenerateJwt(user);
            return Ok(token);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return BadRequest(e.Message);
        }
    }

    [HttpPatch("Logout")]
    public async Task<ActionResult> Logout(UserNameDto dto)
    {
        try
        {
            await userLogic.Logout(dto);
            return Ok();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{username}")]
    public async Task<ActionResult<User>> GetByUsername([FromRoute] string username)
    {
        try
        {
            User user = await userLogic.GetUserByUsernameAsync(username);
            return Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
}