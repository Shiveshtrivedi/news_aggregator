using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.Context
{
    public class NewsDataContextFactory : IDesignTimeDbContextFactory<NewsDataContext>
    {
        public NewsDataContext CreateDbContext(string[] args)
        {
            var basePath = Path.Combine(Directory.GetCurrentDirectory(), "..", "news_aggregator");

            var config = new ConfigurationBuilder()
                .SetBasePath(basePath)
                .AddJsonFile("appsettings.json", optional: false)
                .Build();

            var connectionString = config.GetConnectionString("DefaultConnection");

            var optionsBuilder = new DbContextOptionsBuilder<NewsDataContext>();
            optionsBuilder.UseSqlServer(connectionString);

            return new NewsDataContext(optionsBuilder.Options);
        }
    }
}
