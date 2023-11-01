using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;

namespace HttpClients.Implementations;

public class UserHttpClient : IUserService
{
    private readonly HttpClient _client;
    public static string? Jwt { get; private set; } = "";

    public Action<ClaimsPrincipal> OnAuthStateChanged { get; set; } = null!;
    

    public UserHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<User> CreateAsync(UserNamePasswordDto dto)
    {
        HttpResponseMessage response = await _client.PostAsJsonAsync("/users", dto);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        User user = JsonSerializer.Deserialize<User>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return user;
    }

    public async Task Login(UserNamePasswordDto dto)
    {
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PatchAsync("/Users/Login", body);
        string result = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }
        string token = result;
        Jwt = token;

        ClaimsPrincipal principal = CreateClaimsPrincipal();
        OnAuthStateChanged.Invoke(principal);
    }

    public async Task Logout()
    {
        // Extract the username from the JWT
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        Claim? usernameClaim = principal.FindFirst(ClaimTypes.Name);
        
        string username = usernameClaim?.Value ?? ""; // Get the username

        // Now you can use the username as needed, for example, to include it in the logout request
        UserNameDto dto = new UserNameDto(username);
        string dtoAsJson = JsonSerializer.Serialize(dto);
        StringContent body = new StringContent(dtoAsJson, Encoding.UTF8, "application/json");
        HttpResponseMessage response = await _client.PatchAsync("/Users/Logout", body);

        if (!response.IsSuccessStatusCode)
        {
            string result = await response.Content.ReadAsStringAsync();
            throw new Exception(result);
        }
    
        Jwt = null;
        principal = new ClaimsPrincipal(); // Create an empty ClaimsPrincipal
        OnAuthStateChanged.Invoke(principal);
    }

    public Task<ClaimsPrincipal> GetAuthAsync()
    {
        ClaimsPrincipal principal = CreateClaimsPrincipal();
        return Task.FromResult(principal);    }
    
    private static IEnumerable<Claim> ParseClaimsFromJwt(string jwt)
    {
        string payload = jwt.Split('.')[1];
        byte[] jsonBytes = ParseBase64WithoutPadding(payload);
        Dictionary<string, object>? keyValuePairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
        return keyValuePairs!.Select(kvp => new Claim(kvp.Key, kvp.Value.ToString()!));
    }
    
    private static byte[] ParseBase64WithoutPadding(string base64)
    {
        switch (base64.Length % 4)
        {
            case 2:
                base64 += "==";
                break;
            case 3:
                base64 += "=";
                break;
        }

        return Convert.FromBase64String(base64);
    }
    
    private static ClaimsPrincipal CreateClaimsPrincipal()
    {
        if (string.IsNullOrEmpty(Jwt))
        {
            return new ClaimsPrincipal();
        }

        IEnumerable<Claim> claims = ParseClaimsFromJwt(Jwt);
    
        ClaimsIdentity identity = new(claims, "jwt");

        ClaimsPrincipal principal = new(identity);
        return principal;
    }
    
}