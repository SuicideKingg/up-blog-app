using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp;
using UpBlogApp.Controllers;
using UpBlogApp.DTO;

namespace backend.Tests;

public class CommentControllerTest : TestBase
{
    [Fact]
    public async void EndpointCreateCommentShouldReturnOkStatusWithCorrectDataInsertedData()
    {
         // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        string expectedContentTest = "Test Content";
        int expectedUserIdTest = 1;
        int expectedArticleId = 1;

        CreateUpdateCommentDTO createUpdateArticleDTO = new CreateUpdateCommentDTO()
        {
            Content = expectedContentTest,
            UserID = expectedUserIdTest,
            ArticleID = expectedArticleId
        };

        // Act
        var controller = new CommentController(appDBContext);
        var result = await controller.CreateComment(createUpdateArticleDTO) as OkResult;

        // Assert
        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var actualComment = appDBContext.Comments.AsNoTracking().ToList()[appDBContext.Comments.Count() - 1];

        Assert.Equal(expectedContentTest, actualComment.Content);
        Assert.Equal(expectedUserIdTest, actualComment.UserId);
        Assert.Equal(expectedArticleId, actualComment.ArticleId);
    }
}