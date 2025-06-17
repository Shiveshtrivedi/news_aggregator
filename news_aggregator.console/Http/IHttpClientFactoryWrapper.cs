using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Http
{
    public interface IHttpClientFactoryWrapper
    {
        HttpClient GetClient();
    }
}
