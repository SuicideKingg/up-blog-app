
using backend.Models;
using Microsoft.EntityFrameworkCore;
using UpBlogApp.Models;

namespace UpBlogApp{
    public class AppDBContext : DbContext
    {
        public AppDBContext(DbContextOptions options) : base(options)
        {

        }

        // Seed Data
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            DateTime dateJoined = new DateTime(2025,01,28);
            // Seed User
            modelBuilder.Entity<User>().HasData(
                new User
                {
                    ID=1,
                    DateJoined = dateJoined,
                    Name = "John Doe",
                    Email = "johndoe@example.com",
                    LoginPassword = "1234",
                    UserName = "john_doe",
                    AvaterURL = "avatar1.png",
                    Role = Roles.CLIENT
                },
                new User
                {
                    ID=2,
                    DateJoined = dateJoined,
                    Name = "Mark Herraris",
                    Email = "mark.herraris@example.com",
                    LoginPassword = "1234",
                    UserName = "mark_herraris",
                    AvaterURL = "avatar2.png",
                    Role = Roles.CLIENT
                },
                new User
                {
                    ID = 3,
                    DateJoined = dateJoined,
                    Name = "Jane Wattson",
                    Email = "jw@example.com",
                    LoginPassword = "1234",
                    UserName = "jw_cute_me",
                    AvaterURL = "avatar3.png",
                    Role = Roles.CLIENT
                },
                new User
                {
                    ID = 4,
                    DateJoined = dateJoined,
                    Name = "Kernish Forth",
                    Email = "forthkernish@example.com",
                    LoginPassword = "1234",
                    UserName = "kf_wizard",
                    AvaterURL = "avatar4.png",
                    Role = Roles.CLIENT
                },
                new User
                {
                    ID = 5,
                    DateJoined = dateJoined,
                    Name = "Hendirck Kar",
                    Email = "asxx@example.com",
                    LoginPassword = "1234",
                    UserName = "asxx",
                    AvaterURL = "avatar5.png",
                    Role = Roles.CLIENT
                }
                );

            // Seed Some posts
            modelBuilder.Entity<Article>().HasData(
                new Article
                {
                    ID = 1,
                    Title = "Jingle on 96.3 Easy Rock was changed",
                    Content = "Is there anyone who noticed that the Easy Rock jingle has been chaged. I mean the change was not bad but I still prefer the old ones.",
                    DatePosted = new DateTime(2025,02,05),
                    UserId = 1
                },
                new Article
                {
                    ID = 2,
                    Title = "Is Winforms development a curse when applying to other job?",
                    Content = "I got laid off from my previous company that I worked on and when I try to apply to other job openings for .NET, I usually asked by HR about those ASP.NET things. Im into WPF and WinForms but it seems the industry is seeing this are irrelevant. I always failed on job interview due to lack of work experience into web app.",
                    DatePosted = new DateTime(2025,02,08),
                    UserId = 2
                },
                new Article
                {
                    ID = 3,
                    Title = "Is FPJ Batang Quiapo by Coco Martin is the same as the latter seasons of Probinsyano?",
                    Content = "I don't know where to start but i see slow progress of the story of the series. I see like 'slamdunk' scenario when the freethrow started on monday and the ball will land to the ring on friday. Same thing like on batang quiapo, Olga tried to kill tanggol and let it off on the building on monday and was saved by his mother on friday. I don't understand why they need the story to be expanded by just extening musical socres and zooming effects.",
                    DatePosted = new DateTime(2025,02,10),
                    UserId = 3
                }
                );

            // Seed some comments
            modelBuilder.Entity<Comment>().HasData(
                // Comments for Article 1.
                new Comment
                {
                    ID = 1,
                    Content = "'Slow down and fall in love', yeah I agree i still like the previous ones. Its more relaxing when sleeping or travaling at night on urban places",
                    UserId = 2,
                    DatePosted = new DateTime(2025,02,06),
                    ArticleId = 1
                },
                new Comment
                {
                    ID = 2,
                    Content = "Delilah jingle package is still relaxing for me especially when traveling out of town.",
                    UserId = 3,
                    DatePosted = new DateTime(2025,02,06),
                    ArticleId = 1
                },

                // Comments for Article 2.
                new Comment
                {
                    ID = 3,
                    Content = "I suffered that too. When I left my previous company to expand my career, I always got rejected due to the industry are in favor for experienced web developers when I'm applying.",
                    UserId = 1,
                    DatePosted = new DateTime(2025,02,09),
                    ArticleId = 2
                },
                new Comment
                {
                    ID = 4,
                    Content = "There are still job openings for WinForms or WPF but still be relevant on the modern tech stack. I dont see it as a curse but it could be your challenge when applying for Web Development due to Fullstack devs competition.",
                    UserId = 3,
                    DatePosted = new DateTime(2025,02,09),
                    ArticleId = 2
                },
                new Comment
                {
                    ID = 5,
                    Content = "Its not about being cursed but tough market in Job Openings for Tech which Web Dev are dominant. If where we still on circa 2018 Im pretty sure some companies will give you chance to grow and exposed to web development because the usual volume of applicants are not that high as today. Nowadays in each job opening has more than 100+ applicants. Hring Managers will have a lot of choices that who is skilled that is budget friendly. My final word will be GOOD LUCK on job hunting and DON'T LOSE HOPE",
                    UserId = 4,
                    DatePosted = new DateTime(2025,02,10),
                    ArticleId = 2
                },
                new Comment
                {
                    ID = 6,
                    Content = "I think upskilling and having certifications could beat other candidates with professional experience. I understand most of companies will prefer actual than theory. I'm still on my Desktop app career and wondering if I should leave. I'm considering my current job due to that I got responsibilities(paying bills etc.).",
                    UserId = 5,
                    DatePosted = new DateTime(2025,02,11),
                    ArticleId = 2
                },

                // Comments for Article 3.
                new Comment
                {
                    ID = 7,
                    Content = "The problem there is the story is not constructed well. I understand that having plot twists will boost views. But unsconstructed story will lead to production issues and I think having it delayed is one. Im not sure what Coco really doing but as far as I know he's helping actors/actresses to have a job.",
                    UserId = 4,
                    DatePosted = new DateTime(2025,02,12),
                    ArticleId = 3
                },
                new Comment
                {
                    ID = 8,
                    Content = "Its really too far to the actual film. There is no santino, david, rigor or Red Pheonix. Its just a simple story of a man who got freed from pickpocketing. They are doing is just brainstorming on what will happen next in order to secure the job of the actors",
                    UserId = 5,
                    DatePosted = new DateTime(2025,02,13),
                    ArticleId = 3
                },
                new Comment
                {
                    ID = 9,
                    Content = "Well for Coco martin FPJ series its normal to have slow story progress. But I think the primary reason for making it extended is due to LOVI POE's departure. There was a trailer that was suppose to end Mccoy De Leon's character but due to this, Coco forced to modify the story that make the series go extend. The viewers we're not happy on this.",
                    UserId = 1,
                    DatePosted = new DateTime(2025,02,14),
                    ArticleId = 3
                }
                );

            // Seed some reactions
            modelBuilder.Entity<Reaction>().HasData(
                // Reaction for Article 1
                new Reaction
                {
                    ID =1,
                    ArticleId=1,
                    UserId=2,
                    ReactionType = ReactionType.Like
                },
                new Reaction
                {
                    ID = 2,
                    ArticleId = 1,
                    UserId = 5,
                    ReactionType = ReactionType.Dislike
                },

                // Reaction for Article 2
                new Reaction
                {
                    ID = 3,
                    ArticleId = 2,
                    UserId = 4,
                    ReactionType = ReactionType.Like
                },
                
                // Reaction for Article 3
                new Reaction
                {
                    ID = 4,
                    ArticleId = 3,
                    UserId = 1,
                    ReactionType = ReactionType.Like
                },
                new Reaction
                {
                    ID = 5,
                    ArticleId = 3,
                    UserId = 2,
                    ReactionType = ReactionType.Like
                },
                new Reaction
                {
                    ID = 6,
                    ArticleId = 3,
                    UserId = 4,
                    ReactionType = ReactionType.Dislike
                }
                );
        }

        // DbSets
        public virtual DbSet<Article> Articles { get; set; }
        public virtual DbSet<Comment> Comments { get; set; }
        public virtual DbSet<Reaction> Reactions { get; set; }
        public virtual DbSet<User> User { get; set; }

    }   
}