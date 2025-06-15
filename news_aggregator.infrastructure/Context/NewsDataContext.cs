using Microsoft.EntityFrameworkCore;
using news_aggregator.domain.Models;
using news_application.Models;

namespace news_application.Context
{
    public class NewsDataContext : DbContext
    {
        public NewsDataContext(DbContextOptions<NewsDataContext> options) : base(options) { }

        public DbSet<User> Users { get; set; }
        public DbSet<NewsArticle> NewsArticles { get; set; }
        public DbSet<SavedArticle> SavedArticles { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<ExternalSource> ExternalSources { get; set; }
        public DbSet<NotificationConfig> NotificationConfigs { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<UserKeyword> UserKeywords { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<NewsArticle>()
                .HasOne(a => a.ExternalSource)
                .WithMany(e => e.NewsArticles)
                .HasForeignKey(a => a.ExternalSourceId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}



