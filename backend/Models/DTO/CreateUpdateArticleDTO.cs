namespace UpBlogApp.DTO;
public class CreateUpdateArticleDTO
{
    public int? ID { get; set; }
    public string Title { get; set; }
    public string Content { get; set; }
    public int UserID { get; set; }
}