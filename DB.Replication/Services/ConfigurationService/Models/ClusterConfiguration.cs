namespace ABDDB.Replication.Services
{
    public class ClusterConfiguration
    {
        public ClusterConfiguration(IList<Node> nodes, Node currentNode, int readQuorum = default, int writeQuorum = default)
        {
            Nodes = nodes ?? throw new ArgumentNullException(nameof(nodes));
            CurrentNode = Nodes.FirstOrDefault(n => n == currentNode)
                ?? throw new ArgumentOutOfRangeException(nameof(currentNode));
            OtherNodes = Nodes.Where(n => n != currentNode).ToList();


            if (readQuorum < 0)
                throw new ArgumentOutOfRangeException(nameof(readQuorum));
            if (writeQuorum < 0)
                throw new ArgumentOutOfRangeException(nameof(writeQuorum));
            if (readQuorum == default && writeQuorum == default)
                readQuorum = writeQuorum = nodes.Count / 2 + 1;
            else if (writeQuorum == default)
                writeQuorum = nodes.Count - readQuorum + 1;
            else if (readQuorum == default)
                readQuorum = nodes.Count - writeQuorum + 1;
            if (readQuorum + writeQuorum <= Nodes.Count)
                throw new ArgumentOutOfRangeException(nameof(readQuorum), "Quorums don't overlap");
            ReadQuorum = readQuorum;
            WriteQuorum = writeQuorum;
        }

        public IList<Node> Nodes { get; }
        public IList<Node> OtherNodes { get; }
        public Node CurrentNode { get; }
        public int ReadQuorum { get; }
        public int WriteQuorum { get; }
    }
}