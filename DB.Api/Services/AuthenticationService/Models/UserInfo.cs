namespace ABDDB.Api.Services.Models;

struct UserInfo
{
    public string PasswordHash { get; set; }
    public byte[] Salt { get; set; }
    public Role Role { get; set; }

    public UserInfo(string passwordHash, byte[] salt, Role role)
    {
        PasswordHash = passwordHash;
        Salt = salt;
        Role = role;
    }
}

