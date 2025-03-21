using System.ComponentModel.DataAnnotations;

namespace UpBlogApp.Models
{
    public class User
    {
        [Key]
        public int ID { get; set; }

        public string Name { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public DateTime DateJoined { get; set; }
        public string AvaterURL { get; set; }

        public string LoginPassword { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
        public string Role { get; set; } // 'Admin' or 'User'

        #region Navigation Properties
        public ICollection<Article> Articles { get; set; }
        public ICollection<Comment> Comments { get; set; }
        public ICollection<Reaction> Reactions { get; set; }
        #endregion
    }
}