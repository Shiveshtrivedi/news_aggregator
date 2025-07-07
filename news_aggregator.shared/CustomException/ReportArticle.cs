using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.shared.CustomException
{
    public class ReportAddException : Exception
    {
        public ReportAddException(string message, Exception inner) : base(message, inner) { }
    }

    public class ReportCountFetchException : Exception
    {
        public ReportCountFetchException(string message, Exception inner) : base(message, inner) { }
    }

    public class UserReportCheckException : Exception
    {
        public UserReportCheckException(string message, Exception inner) : base(message, inner) { }
    }

    public class ReportProcessException : Exception
    {
        public ReportProcessException(string message, Exception inner) : base(message, inner) { }
    }
}
