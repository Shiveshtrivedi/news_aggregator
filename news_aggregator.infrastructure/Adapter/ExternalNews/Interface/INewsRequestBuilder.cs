using news_aggregator.domain.Models.DTOs;
using news_application.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.Adapter.ExternalNews.Interface
{
    public interface INewsRequestBuilder
    {
        HttpRequestMessage BuildRequest(ExternalSourceDto source, string category, string keyword);
    }

}
