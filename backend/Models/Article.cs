using System.ComponentModel.DataAnnotations;

namespace UpBlogApp.Models
{
    public class Article
    {
         [Key]
        public int ID { get; set; }

        public DateTime DatePosted { get; set; }
        public required string Title { get; set; }
        public string Content { get; set; }

         #region Navigation Properties
        public int? UserId { get; set; }
        public User User { get; set; }

        // Comments
        public ICollection<Comment> Comments { get; set; }

        // Reactions
        public ICollection<Reaction> Reactions { get; set; }
        #endregion
    }
}