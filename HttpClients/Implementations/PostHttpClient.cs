using System.Net.Http.Json;
using System.Security.Claims;
using System.Text.Json;
using Domain.DTOs;
using Domain.Models;
using HttpClients.ClientInterfaces;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;


namespace HttpClients.Implementations;

public class PostHttpClient : IPostService
{
    private readonly HttpClient _client;

    public PostHttpClient(HttpClient client)
    {
        _client = client;
    }
    
    public async Task<Post> CreateAsync(PostCreationDto dto)
    {
        if (string.IsNullOrEmpty(UserHttpClient.Jwt))
        {
            // Handle the case where the user is not authenticated
            throw new Exception("User is not authenticated. Please log in.");
        }

        // Add the JWT token to the request headers
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", UserHttpClient.Jwt);

        HttpResponseMessage response = await _client.PostAsJsonAsync("/Posts", dto);
        string result = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(result);
        }

        Post post = JsonSerializer.Deserialize<Post>(result, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return post;
    }

    public async Task<IEnumerable<Post>> GetPosts()
    {
        HttpResponseMessage response = await _client.GetAsync("/Posts");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(content);
        }

        IEnumerable<Post> posts = JsonSerializer.Deserialize<IEnumerable<Post>>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return posts;
    }

    public async Task<Post> GetById(int id)
    {
        HttpResponseMessage response = await _client.GetAsync($"/Posts/{id}");
        string content = await response.Content.ReadAsStringAsync();
        if (!response.IsSuccessStatusCode)
            throw new Exception(content);
        Post post = JsonSerializer.Deserialize<Post>(content, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        })!;
        return post;
    }
}