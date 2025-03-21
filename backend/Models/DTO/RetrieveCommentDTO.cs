namespace UpBlogApp.DTO;
public class RetrieveCommentDTO
{
    public int ID { get; set; }
    public string Content { get; set; }
    public DateTime DatePosted { get; set; }
    public int AuthorID { get; set; }
    public string Author { get; set; }
    public string AuthorURLAvatar { get; set; }
}