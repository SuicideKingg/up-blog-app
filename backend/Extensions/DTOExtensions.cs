using backend.Models;
using UpBlogApp.DTO;
using UpBlogApp.Models;

namespace UpBlogApp.Extensions
{
    public static class DTOExtensions
    {
        public static Article ToArticleEntity(this CreateUpdateArticleDTO createUpdateArticleDTO)
        {
            return new Article()
            {
                ID = GetIdOrZero(createUpdateArticleDTO.ID),
                Title = createUpdateArticleDTO.Title,
                Content = createUpdateArticleDTO.Content,
                DatePosted = DateTime.Now,
                UserId = createUpdateArticleDTO.UserID
            };
        }

        public static Comment ToCommentEntity(this CreateUpdateCommentDTO createUpdateCommentDTO)
        {
            return new Comment()
            {
                ID = GetIdOrZero(createUpdateCommentDTO.ID),
                ArticleId = createUpdateCommentDTO.ArticleID,
                Content = createUpdateCommentDTO.Content,
                DatePosted = DateTime.Now,
                UserId = createUpdateCommentDTO.UserID
            };
        }

        public static Reaction ToReactionEntity(this CreateUpdateReactionDTO createUpdateReactionDTO)
        {
            return new Reaction()
            {
                ID = GetIdOrZero(createUpdateReactionDTO.ID),
                ArticleId = createUpdateReactionDTO.ArticleID,
                ReactionType = createUpdateReactionDTO.ReactionType,
                UserId = createUpdateReactionDTO.UserID
            };
        }

        public static User ToUserEntity(this RegisterModel registerModel)
        {
            return new User()
            {
                ID = GetIdOrZero(registerModel.ID),
                AvaterURL = registerModel.AvaterURL,
                DateJoined = DateTime.Now,
                Email = registerModel.Email,
                LoginPassword = registerModel.LoginPassword,
                Name = registerModel.Name,
                Role = Roles.CLIENT,
                UserName = registerModel.UserName
            };
        }

        private static int GetIdOrZero(int? id)
        {
            return id == null ? 0 : (int)id;
        }
    }
}