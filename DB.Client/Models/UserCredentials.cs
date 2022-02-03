namespace ABDDB.Client.Models
{
    public record UserCredentials
    {
        public UserCredentials(string userName, string password)
        {
            UserName = !string.IsNullOrWhiteSpace(userName) ? userName
                   : throw new ArgumentNullException(nameof(userName));

            Password = !string.IsNullOrWhiteSpace(password) ? password
                   : throw new ArgumentNullException(nameof(password));
        }

        public string UserName { get; }
        public string Password { get; }
    }
}