using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException
{
    public class SaveArticleFailedException : Exception
    {
        public SaveArticleFailedException(string message, Exception inner) : base(message, inner) { }
    }

    public class GetSavedArticlesFailedException : Exception
    {
        public GetSavedArticlesFailedException(string message, Exception inner) : base(message, inner) { }
    }

    public class DeleteSavedArticleFailedException : Exception
    {
        public DeleteSavedArticleFailedException(string message, Exception inner) : base(message, inner) { }
    }
}
