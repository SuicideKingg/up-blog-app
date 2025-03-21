using UpBlogApp.Models;

namespace UpBlogApp.DTO;
public class CreateUpdateReactionDTO
{
    public int? ID { get; set; }
    public ReactionType ReactionType { get; set; }

    public int ArticleID { get; set; }
    public int UserID { get; set; }
}