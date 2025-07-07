using Castle.Core.Logging;
using Microsoft.Extensions.Logging;
using Moq;

namespace news_aggregator.tests.TestUtils
{
    public static class FakeLogger<T>
    {
        public static ILogger<T> Create()
        {
            return new Mock<ILogger<T>>().Object;
        }
    }
}
