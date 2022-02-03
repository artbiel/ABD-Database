using Microsoft.Extensions.DependencyInjection;

namespace ABDDB.Replication.Transport
{
    public class LocalChannel : IChannel
    {
        private readonly Node _node;
        private readonly IServiceProvider _serviceProvider;

        public LocalChannel(Node node, IServiceProvider serviceProvider)
        {
            _node = node ?? throw new ArgumentNullException(nameof(node));
            _serviceProvider = serviceProvider ?? throw new ArgumentNullException(nameof(serviceProvider));
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetRequiredService<T>();
        }

        public void Dispose() { }

        public Node Node => _node;
    }
}