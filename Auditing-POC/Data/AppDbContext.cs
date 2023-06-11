using Audit.EntityFramework;
using Auditing_POC.Models;
using Microsoft.EntityFrameworkCore;

namespace Auditing_POC.Data;

public sealed class AppDbContext : AuditDbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
    }

    public DbSet<Blog> Blogs { get; set; }
    
    public DbSet<Post> Posts { get; set; }
}
