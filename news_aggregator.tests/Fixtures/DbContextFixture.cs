using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using news_application.Context;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Fixtures
{
    public class DbContextFixture : IDisposable
    {
        public NewsDataContext Context { get; private set; }

        public DbContextFixture()
        {
            var inMemorySettings = new Dictionary<string, string>
            {
                { "Admin:UserId", "1" },
                { "Admin:UserName", "admin" },
                { "Admin:Email", "admin@example.com" },
                { "Admin:Password", "admin123" }
            };

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection(inMemorySettings)
                .Build();

            var options = new DbContextOptionsBuilder<NewsDataContext>()
                .UseInMemoryDatabase(databaseName: $"TestDb_{Guid.NewGuid()}")
                .Options;

            Context = new NewsDataContext(options, configuration);
            Context.Database.EnsureCreated();
        }

        public void Dispose()
        {
            Context.Database.EnsureDeleted();
            Context.Dispose();
        }
    }
}
