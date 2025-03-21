using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UpBlogApp;
using UpBlogApp.Controllers;
using UpBlogApp.DTO;
using UpBlogApp.Models;

namespace backend.Tests;

public class ArticleTest : TestBase
{
    [Fact]
    public async void EndpointGetAllShouldReturnOkResultsWithCorrectList()
    {
        // Arrange
        var controller = new ArticleController(GetAppDBContext());

        // Act
        var result = await controller.GetAll() as ObjectResult;

        // Assert
        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var list  = result.Value as List<RetrieveArticleDTO>;

        Assert.True(list != null);
        Assert.True(list.Count == 3);
    }

    [Fact]
    public async void EndpointGetSingleArticleShouldReturnOkResultAndCorrectValue()
    {
        // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        var expectedArticle = appDBContext.Articles.AsNoTracking().ToList()[0];
        int articleId = expectedArticle.ID;

        // Act
        var controller = new ArticleController(appDBContext);
        var result = await controller.GetSingleArticle(articleId) as ObjectResult;

        // Assert
        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var actualArticle = result.Value as RetrieveArticleDTO;

        Assert.True(actualArticle != null);
        Assert.Equal(expectedArticle.ID, actualArticle.ID);
        Assert.Equal(expectedArticle.Title, actualArticle.Title);
        Assert.Equal(expectedArticle.Content, actualArticle.Content);
        Assert.Equal(expectedArticle.DatePosted, actualArticle.DatePosted);
        Assert.Equal(expectedArticle.UserId, actualArticle.AuthorID);
    }

    [Fact]
    public async void EndpointCreateArticleShouldCreateEntitySuccesfully()
    {
        // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        var controller = new ArticleController(appDBContext);
        string expectedTitleTest = "Test Article";
        string expectedContentTest = "Test Content";
        int expectedUserIdTest = 1;

        CreateUpdateArticleDTO createUpdateArticleDTO = new CreateUpdateArticleDTO()
        {
            Title = expectedTitleTest,
            Content = expectedContentTest,
            UserID = expectedUserIdTest
        };

        // Act
        var result = await controller.CreateArticle(createUpdateArticleDTO) as OkResult;

        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var actualArticle = appDBContext.Articles.ToList()[appDBContext.Articles.Count() - 1];

        Assert.Equal(expectedTitleTest, actualArticle.Title);
        Assert.Equal(expectedContentTest, actualArticle.Content);
        Assert.Equal(expectedUserIdTest, actualArticle.UserId);
    }

    [Fact]
    public async void EndpointUpdateArticleShouldUpdateEntitySuccesfullya()
    {
        // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        string expectedTitleTest = "Test Article";
        string expectedContentTest = "Test Content";

        var articleToUpdate = appDBContext.Set<Article>().AsNoTracking().FirstOrDefault();
        if(articleToUpdate == null)
        {
            throw new Exception("Article to test is null!");
        }
        int articleId = articleToUpdate.ID;

        CreateUpdateArticleDTO createUpdateArticleDTO;

        if(articleToUpdate.UserId != null)
        {
            createUpdateArticleDTO = new CreateUpdateArticleDTO()
            {
                ID = articleToUpdate.ID,
                Title = expectedTitleTest,
                Content = expectedContentTest,
                UserID = (int)articleToUpdate.UserId
            };
        }
        else
        {
            throw new Exception("User Id is null!");
        }

        var controller = new ArticleController(appDBContext);

        // Act
        var result = await controller.UpdateArticle(createUpdateArticleDTO) as OkResult;
        
        // Assert
        Assert.True(result != null);
        Assert.Equal(200, result.StatusCode);

        var updatedArticle = appDBContext.Set<Article>().Where(s => s.ID == articleId).AsNoTracking().FirstOrDefault();
        
        Assert.True(updatedArticle != null);
        Assert.Equal(expectedTitleTest, updatedArticle.Title);
        Assert.Equal(expectedContentTest, updatedArticle.Content);
    }

    [Fact]
    public async void EndpointDeleteArticleShouldRemoveTheEntitySuccesfully()
    {
        // Arrange
        AppDBContext appDBContext = GetAppDBContext();
        int initialCountOfArticles = appDBContext.Articles.Count();
        int articleTestId = 1;

        var controller = new ArticleController(appDBContext);

        // Act
        var results = await controller.DeleteArticle(articleTestId) as OkResult;

        // Assert
        Assert.True(results != null);
        Assert.Equal(initialCountOfArticles - 1, appDBContext.Articles.Count());
        var removedArticle = appDBContext.Set<Article>().Where(s => s.ID == articleTestId).AsNoTracking().FirstOrDefault();
        Assert.True(removedArticle == null);
    }
   
}