using Application.DaoInterfaces;
using Domain.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace EfcDataAccess.DAOs;
public class PostEfcDao : IPostDao
{
    private readonly PostContext postContext;

    public PostEfcDao(PostContext postContext)
    {
        this.postContext = postContext;
    }

    public async Task<Post> CreateAsync(Post post)
    {
        EntityEntry<Post> added = await postContext.Posts.AddAsync(post);
        await postContext.SaveChangesAsync();
        return added.Entity;
    }

    public async Task<IEnumerable<Post?>> GetAsync()
    {
        IQueryable<Post> postsQuery = postContext.Posts.AsQueryable();
        IEnumerable<Post> result = await postsQuery.ToListAsync();
        return result;

    }

    public async Task<Post?> GetByIdAsync(int id)
    {
        Post? found = await postContext.Posts
            .Include(post => post.Owner)
            .SingleOrDefaultAsync(post => post.Id == id);
        return found;
    }
}