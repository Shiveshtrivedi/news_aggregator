using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.application
{
    public interface INewsProviderFactory
    {
        INewsProvider GetProvider(string sourceName);
    }
}
