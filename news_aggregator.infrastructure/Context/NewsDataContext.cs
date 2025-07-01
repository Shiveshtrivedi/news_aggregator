using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using news_aggregator.domain.Models;
using news_application.Enum;
using news_application.Models;

namespace news_application.Context
{
    public class NewsDataContext : DbContext
    {
        private readonly IConfiguration _configuration;
        public NewsDataContext(DbContextOptions<NewsDataContext> options, IConfiguration configuration) : base(options) 
        {
            _configuration = configuration;
        }

        public DbSet<User> Users { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ExternalSource> ExternalSources { get; set; }
        public DbSet<NotificationConfig> NotificationConfigs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserKeyword> UserKeywords { get; set; }
        public DbSet<UserArticleInteraction> UserArticleInteractions { get; set; }
        public DbSet<ReportArticle> ReportArticles { get; set; }
        public DbSet<BlockedKeyword> BlockedKeywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsArticle>()
                .HasOne(a => a.ExternalSource)
                .WithMany(e => e.NewsArticles)
                .HasForeignKey(a => a.ExternalSourceId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<User>().HasData(
                new User
                {
                    UserId = int.Parse(_configuration["Admin:UserId"]),
                    UserName = _configuration["Admin:UserName"],
                    Email = _configuration["Admin:Email"],
                    Password = _configuration["Admin:Password"],
                    Role = UserRole.Admin,
                    IsTokenActive = false,
                    RefreshToken = null,
                    RefreshTokenExpiryTime = null
                }
            );
            modelBuilder.Entity<Category>().HasData(
                new Category
                {
                    CategoryId = 1,
                    CategoryName = "business"
                },
                new Category
                {
                    CategoryId = 2,
                    CategoryName = "entertainment"
                },
                 new Category
                 {
                     CategoryId = 3,
                     CategoryName = "sports"
                 },
                  new Category
                  {
                      CategoryId = 4,
                      CategoryName = "uncategorized"
                  },
                  new Category
                  {
                      CategoryId = 5,
                      CategoryName = "technology"
                  }
            );

        }
    }
}



