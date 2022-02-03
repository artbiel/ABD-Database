namespace ABDDB.Replication.Transport
{
    public interface IChannelPool : IDisposable
    {
        IChannel? GetChannel(Node Node);
        IEnumerable<IChannel> GetChannels();
        IChannel? this[Node node] { get; }
    }
}