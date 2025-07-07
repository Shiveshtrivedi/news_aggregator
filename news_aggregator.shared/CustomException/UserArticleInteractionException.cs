using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException
{
    public class UserArticleInteractionException : Exception
    {
        public UserArticleInteractionException(string message, Exception inner) : base(message, inner) { }
    }
}
