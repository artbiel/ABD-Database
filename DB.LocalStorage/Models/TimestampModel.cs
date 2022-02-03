namespace ABDDB.LocalStorage.Models
{
    public struct TimestampModel : IComparable<TimestampModel>
    {
        public long Timestamp { get; private set; }
        public Guid Salt { get; private set; }

        public TimestampModel(long timestamp, Guid salt)
        {
            Timestamp = timestamp;
            Salt = salt;
        }

        public static bool operator >(TimestampModel first, TimestampModel second) =>
            first.CompareTo(second) > 0;

        public static bool operator <(TimestampModel first, TimestampModel second) =>
           first.CompareTo(second) < 0;

        public int CompareTo(TimestampModel other) =>
            Timestamp != other.Timestamp ? (int)(Timestamp - other.Timestamp) :
                Salt.CompareTo(other.Salt);
    }
}