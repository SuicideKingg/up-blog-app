using UpBlogApp.Models;

namespace UpBlogApp.DTO;
public class RetrieveReactionDTO
{
    public int ID { get; set; }
    public ReactionType ReactionType { get; set; }
    public int AuthorID { get; set; }
    public string Author { get; set; }
    public string AuthorURLAvatar { get; set; }
}