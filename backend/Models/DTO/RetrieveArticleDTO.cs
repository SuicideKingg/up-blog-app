namespace UpBlogApp.DTO;
public class RetrieveArticleDTO
{
    public int ID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public string Author { get; set; }
    public int AuthorID { get; set; }
    public string AuthorAvatarURL { get; set; }

    public List<RetrieveCommentDTO> Comments { get; set; }
    public List<RetrieveReactionDTO> Reactions { get; set; }
}