using news_aggregator.domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.tests.Helpers
{
    public static class UserKeywordMockData
    {
        public static List<UserKeyword> GetUserKeywords() =>
            new()
            {
                new UserKeyword { UserId = 1, Keyword = "AI" },
                new UserKeyword { UserId = 1, Keyword = "Health" }
            };
    }
}
