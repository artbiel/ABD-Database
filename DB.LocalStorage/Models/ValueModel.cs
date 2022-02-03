namespace ABDDB.LocalStorage.Models
{
    public struct ValueModel
    {
        public string Value { get; }
        public TimestampModel Timestamp { get; }

        public ValueModel(string value, TimestampModel timestamp)
        {
            Value = value;
            Timestamp = timestamp;
        }

        public void Deconstruct(out string value, out TimestampModel timestamp)
        {
            value = Value;
            timestamp = Timestamp;
        }
    };
}