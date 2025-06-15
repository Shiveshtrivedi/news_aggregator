using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.infrastructure.ExternalApi
{
    public class AltNewsApiResponse
    {
        public MetaData Meta { get; set; }
        public List<AltNewsArticle> Data { get; set; }
    }

    public class MetaData
    {
        public int Found { get; set; }
        public int Returned { get; set; }
        public int Limit { get; set; }
        public int Page { get; set; }
    }

    public class AltNewsArticle
    {
        public string Uuid { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Snippet { get; set; }
        public string Url { get; set; }
        public string Image_Url { get; set; }
        public DateTime Published_At { get; set; }
        public string Source { get; set; }
    }

}
