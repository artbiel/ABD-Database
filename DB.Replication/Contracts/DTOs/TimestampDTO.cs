using ABDDB.LocalStorage.Models;
using System.Runtime.Serialization;

namespace ABDDB.Replication.Contracts.DTOs
{
    [DataContract]
    public struct TimestampDTO
    {
        [DataMember(Order = 1)]
        public long Timestamp { get; set; }

        [DataMember(Order = 2)]
        public Guid Salt { get; set; }

        public TimestampDTO(TimestampModel model)
        {
            Timestamp = model.Timestamp;
            Salt = model.Salt;
        }

        public static implicit operator TimestampDTO(TimestampModel model) => new(model);

        public static implicit operator TimestampModel(TimestampDTO dto) => new(dto.Timestamp, dto.Salt);
    }
}