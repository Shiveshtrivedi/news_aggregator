using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException.CategoryException
{
    public class InvalidCategoryException : Exception
    {
        public InvalidCategoryException(string message) : base(message) { }
    }
}
