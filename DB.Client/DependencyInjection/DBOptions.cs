using Microsoft.Extensions.Logging;

namespace ABDDB.Client
{
    public class DBOptions
    {
        internal ILoggerFactory LoggerFactory { get; private set; }
            = Microsoft.Extensions.Logging.LoggerFactory.Create(options => options.AddConsole());

        public void UseLoggerFactory(ILoggerFactory loggerFactory) =>
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
    }
}