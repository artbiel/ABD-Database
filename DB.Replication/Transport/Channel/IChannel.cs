namespace ABDDB.Replication.Transport
{
    public interface IChannel : IDisposable
    {
        T GetService<T>() where T : class;
        Node Node { get; }
    }
}
