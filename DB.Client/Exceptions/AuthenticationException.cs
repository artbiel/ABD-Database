namespace ABDDB.Client.Exceptions
{
    public class AuthenticationException : ADBDBExeption
    {
        public AuthenticationException() : base("Incorrect username or password") { }
    }
}