using Microsoft.EntityFrameworkCore;
using UpBlogApp;

public abstract class TestBase
{
    protected AppDBContext GetAppDBContext()
    {
         var options = new DbContextOptionsBuilder<AppDBContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()) // Unique DB for each test
            .Options;

        var context = new AppDBContext(options);
        context.Database.EnsureCreated(); // Ensure the database schema is created

        // NOTE: Nothing to seed here. The data to be used was on the OnModelCreating overriden method of the AppDBContext.

        return context;
    }
}