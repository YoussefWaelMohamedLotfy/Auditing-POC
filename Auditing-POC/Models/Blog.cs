namespace Auditing_POC.Models;

public class Blog
{
    public int ID { get; set; }

    public string Title { get; set; }

    public string Text { get; set; }

    public List<Post>? Posts { get; set; }
}
