namespace Auditing_POC.Models;

public class Post
{
    public int ID { get; set; }

    public string Title { get; set; }

    public int BlogId { get; set; }

    public Blog Blog { get; set; }
}
