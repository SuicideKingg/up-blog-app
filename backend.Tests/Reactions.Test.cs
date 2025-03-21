using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp;
using UpBlogApp.Controllers;
using UpBlogApp.DTO;
using UpBlogApp.Models;

namespace backend.Tests;

public class ReactionControllerTest : TestBase
{
    [Fact]
    public async void EndpointCreateReactionShouldReturnOkStatusWithCorrectDataInsertedData()
    {
         // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        ReactionType expectedReaction = ReactionType.Like;
        int expectedUserIdTest = 1;
        int expectedArticleId = 1;

        CreateUpdateReactionDTO createUpdateReactionDTO = new CreateUpdateReactionDTO()
        {
            ReactionType = ReactionType.Like,
            UserID = expectedUserIdTest,
            ArticleID = expectedArticleId
        };

        // Act
        var controller = new ReactionController(appDBContext);
        var result = await controller.CreateReaction(createUpdateReactionDTO) as OkResult;

        // Assert
        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var actualReaction = appDBContext.Reactions.AsNoTracking().ToList()[appDBContext.Reactions.Count() - 1];

        Assert.Equal(expectedReaction, actualReaction.ReactionType);
        Assert.Equal(expectedUserIdTest, actualReaction.UserId);
        Assert.Equal(expectedArticleId, actualReaction.ArticleId);
    }

    [Fact]
    public async void EndpointDeleteReactionShouldRemoveTheEntitySuccesfully()
    {
        // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        int initialCountOfReactions = appDBContext.Reactions.Count();
        int reactionIdTest = 1;

        var controller = new ReactionController(appDBContext);

        // Act
        var results = await controller.RemoveReaction(reactionIdTest) as OkResult;

        // Assert
        Assert.True(results != null);
        Assert.Equal(initialCountOfReactions - 1, appDBContext.Reactions.Count());
        var removedReaction = appDBContext.Set<Reaction>().Where(s => s.ID == reactionIdTest).AsNoTracking().FirstOrDefault();
        Assert.True(removedReaction == null);
    }
}