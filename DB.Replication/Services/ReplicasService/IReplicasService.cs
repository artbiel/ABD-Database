using ABDDB.Replication.Contracts;

namespace ABDDB.Replication.Services
{
    public interface IReplicasService
    {
        IEnumerable<IReplica> GetReplicas();
    }
}
