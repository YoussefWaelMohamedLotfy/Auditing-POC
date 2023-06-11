using Auditing_POC.Data;
using Auditing_POC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Auditing_POC.Controllers;

[Route("api/[controller]")]
[ApiController]
public class BlogsController : ControllerBase
{
    private readonly AppDbContext _context;

    public BlogsController(AppDbContext context)
    {
        _context = context;
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
    [HttpPut("{id}")]
    public async Task<IActionResult> PutBlog(int id, Blog blog)
    {
        if (id != blog.ID)
        {
            return BadRequest();
        }

        _context.Update(blog);

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!BlogExists(id))
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
            return Problem("Entity set 'AppDbContext.Blogs'  is null.");
        }
        _context.Blogs.Add(blog);
        await _context.SaveChangesAsync();

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

        return NoContent();
    }

    private bool BlogExists(int id)
    {
        return (_context.Blogs?.Any(e => e.ID == id)).GetValueOrDefault();
    }
}
