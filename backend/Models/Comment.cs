using System.ComponentModel.DataAnnotations;

namespace UpBlogApp.Models
{
    public class Comment
    {
        [Key]
        public int ID { get; set; }

        public DateTime DatePosted { get; set; }
        public string Content { get; set; }

        #region Navigation Properties
        // Author
        public int? UserId { get; set; }
        public User User { get; set; }

        // Comment for What article
        public int? ArticleId { get; set; }
        public Article Article { get; set; }
        #endregion
    }
}