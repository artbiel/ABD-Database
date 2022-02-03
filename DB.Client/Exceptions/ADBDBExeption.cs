namespace ABDDB.Client.Exceptions
{
    public class ADBDBExeption : Exception
    {
        public ADBDBExeption() : base() { }

        public ADBDBExeption(string message) : base(message) { }

        public ADBDBExeption(string message, Exception innerException) : base(message, innerException) { }
    }
}