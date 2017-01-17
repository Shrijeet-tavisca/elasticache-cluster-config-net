#if CORE_CLR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;

namespace Amazon.ElastiCacheCluster
{
    internal class NullLogger<T> : ILogger<T>
    {
        public static readonly ILogger<T> Instance = new NullLogger<T>();

        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
        }

        public bool IsEnabled(LogLevel logLevel) => false;

        public IDisposable BeginScope<TState>(TState state) => Disposable.Instance;

        private class Disposable : IDisposable
        {
            public static IDisposable Instance = new Disposable();
            public void Dispose()
            {
            }
        }
    }
}
#endif
