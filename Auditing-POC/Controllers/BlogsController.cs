using Auditing_POC.Data;
using Auditing_POC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auditing_POC.Controllers;

[Route("api/[controller]")]
[ApiController]
public sealed class BlogsController : ControllerBase
{
    private readonly AppDbContext _context;
    private readonly ILogger<BlogsController> _logger;

    public BlogsController(AppDbContext context, ILogger<BlogsController> logger)
    {
        _context = context;
        _logger = logger;
    }

    // GET: api/Blogs
    [HttpGet]
    public async Task<ActionResult<IEnumerable<Blog>>> GetBlogs()
    {
        if (_context.Blogs == null)
        {
            return NotFound();
        }
        return await _context.Blogs.ToListAsync();
    }

    // GET: api/Blogs/5
    [HttpGet("{id}")]
    public async Task<ActionResult<Blog>> GetBlog(int id)
    {
        if (_context.Blogs == null)
        {
            return NotFound();
        }
        var blog = await _context.Blogs.FindAsync(id);

        if (blog == null)
        {
            return NotFound();
        }

        return blog;
    }

    // PUT: api/Blogs/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut]
    public async Task<IActionResult> PutBlog(BlogVM blog)
    {
        var blogInDb = await _context.Blogs.FindAsync(blog.ID);

        if (blogInDb is null)
        {
            return NotFound();
        }

        blogInDb.Title = blog.Title;
        blogInDb.Text = blog.Text;
        _context.Update(blogInDb);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BlogExists(blog.ID))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return NoContent();
    }

    // POST: api/Blogs
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost]
    public async Task<ActionResult<Blog>> PostBlog(Blog blog)
    {
        if (_context.Blogs == null)
        {
            return Problem("Entity set 'AppDbContext.Blogs' is null.");
        }
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Added new Post with blog id {blogId}", blog.ID);
        return CreatedAtAction("GetBlog", new { id = blog.ID }, blog);
    }

    // DELETE: api/Blogs/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteBlog(int id)
    {
        if (_context.Blogs == null)
        {
            return NotFound();
        }
        var blog = await _context.Blogs.FindAsync(id);
        if (blog == null)
        {
            return NotFound();
        }

        _context.Blogs.Remove(blog);
        await _context.SaveChangesAsync();

        _logger.LogInformation("Deleted Post with blog id {blogId}", blog.ID);
        return NoContent();
    }

    private bool BlogExists(int id)
    {
        return (_context.Blogs?.Any(e => e.ID == id)).GetValueOrDefault();
    }
}
