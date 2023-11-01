using Application.LogicInterfaces;
using Domain.DTOs;
using Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace DNP1_Assignment.Controllers;

[ApiController]
[Route("[controller]")]
[Authorize]
public class PostsController : ControllerBase
{
    private readonly IPostLogic postLogic;

    public PostsController(IPostLogic postLogic)
    {
        this.postLogic = postLogic;
    }
    
    [HttpPost]
    public async Task<ActionResult<Post>>? CreateAsync(PostCreationDto dto)
    {
        try
        {
            Post post = await postLogic.CreateAsync(dto);
            return Created($"/posts/{post.Id}", post);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet, AllowAnonymous]
    public async Task<ActionResult<IEnumerable<Post>>> GetAsync()
    {
        try
        {
            IEnumerable<Post> posts = await postLogic.GetPosts();
            return Ok(posts);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }

    [HttpGet("{id:int}"), AllowAnonymous]
    public async Task<ActionResult<Post>> GetByIdAsync([FromRoute] int id)
    {
        try
        {
            Post post = await postLogic.GetByIdAsync(id);
            return Ok(post);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            return StatusCode(500, e.Message);
        }
    }
    
}