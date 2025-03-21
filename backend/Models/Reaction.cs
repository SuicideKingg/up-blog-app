using System.ComponentModel.DataAnnotations;

namespace UpBlogApp.Models
{
    public class Reaction
    {
        [Key]
        public int ID { get; set; }

        public ReactionType ReactionType { get; set; }

        #region Navigation Properties        
        // Author of that Reaction
        public int? UserId { get; set; }
        public User User { get; set; }

        // Article
        public int? ArticleId { get; set; }
        public Article Article { get; set; }
        #endregion
    }

    public enum ReactionType : int
    {
        Like = 1,
        Dislike = 2
    }
}