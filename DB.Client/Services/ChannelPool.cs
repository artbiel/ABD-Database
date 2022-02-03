using ABDDB.Client.Exceptions;
using ABDDB.Client.Models;
using ABDDB.Shared.Client;
using ABDDB.Shared.Client.Messages;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Logging;
using ProtoBuf.Grpc.Client;

namespace ABDDB.Client.Services
{
    internal class ChannelPool : IChannelPool
    {
        private readonly string[] _nodes;
        private readonly List<string> _activeNodes;
        private GrpcChannel _activeChannel;
        private string _currentNode;

        private readonly UserCredentials _credentials;

        private readonly ILogger<ChannelPool> _logger;

        public ChannelPool(ILoggerFactory loggerFactory, string connectionString, UserCredentials credentials)
        {
            if (string.IsNullOrEmpty(connectionString))
                throw new ArgumentNullException(nameof(connectionString));
            _nodes = connectionString.Split(';').Select(u => new Uri(u).OriginalString).ToArray();
            _activeNodes = _nodes.ToList();

            _credentials = credentials ?? throw new ArgumentNullException(nameof(credentials));

            _logger = loggerFactory?.CreateLogger<ChannelPool>() ?? throw new ArgumentNullException(nameof(loggerFactory));
        }

        public GrpcChannel GetActiveChannel() => _activeChannel;

        public async Task Execute(Func<GrpcChannel, Task> task)
        {
            try
            {
                if (_activeChannel is null)
                    await Connect();
                await task(_activeChannel);
            }
            catch (RpcException)
            {
                if (_currentNode is not null)
                    _activeNodes.Remove(_currentNode);
                _activeChannel = null;
                _currentNode = null;
                await Execute(task);
            }
        }

        public async Task<T> Execute<T>(Func<GrpcChannel, Task<T>> task)
        {
            try
            {
                if (_activeChannel is null)
                    await Connect();
                return await task(_activeChannel);
            }
            catch (RpcException ex)
            {
                if (_currentNode is not null)
                    _activeNodes.Remove(_currentNode);
                _activeChannel = null;
                _currentNode = null;
                return await Execute(task);
            }
        }

        private async Task Connect()
        {
            if (!_activeNodes.Any())
                throw new ChannelException("No active nodes");
            var node = Random.Shared.Next(_activeNodes.Count);
            _currentNode = _activeNodes[node];
            _logger.LogDebug($"Connecting to {_currentNode}...");
            var newChannel = GrpcChannel.ForAddress(_currentNode);
            var clientService = newChannel.CreateGrpcService<IClientService>();
            var authenticatedResponse = await clientService
                .Connect(new ConnectRequest { UserName = _credentials.UserName, Password = _credentials.Password });
            if (!authenticatedResponse.Authenticated)
                throw new AuthenticationException();
            _activeChannel = newChannel;
            _logger.LogDebug($"Connected to {_currentNode}");
        }

        public async Task Disconnect()
        {
            var clientService = _activeChannel.CreateGrpcService<IClientService>();
            await clientService.Disconnect();
            await _activeChannel.ShutdownAsync();
        }

        public async ValueTask DisposeAsync() => await Disconnect();
    }
}