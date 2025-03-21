namespace UpBlogApp.DTO;
public class CreateUpdateCommentDTO
{
    public int? ID { get; set; }
    public string Content { get; set; }
    public int ArticleID { get; set; }
    public int UserID { get; set; }
}