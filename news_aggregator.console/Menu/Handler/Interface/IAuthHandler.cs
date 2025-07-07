using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace news_aggregator.console.Menu.Handler.Interface
{
    public interface IAuthHandler
    {
        Task HandleSignUpAsync();
        Task HandleLoginAsync();
    }
}
