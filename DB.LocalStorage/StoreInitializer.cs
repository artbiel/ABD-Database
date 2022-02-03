using ABDDB.LocalStorage.Models;
using Microsoft.AspNetCore.Cryptography.KeyDerivation;
using System.Security.Cryptography;
using System.Text.Json;

namespace ABDDB.LocalStorage
{
    public static class StoreInitializer
    {
        public static IEnumerable<(string, ValueModel)> GetInitialValues()
        {
            byte[] salt = RandomNumberGenerator.GetBytes(128 / 8);

            string passwordHash = Convert.ToBase64String(KeyDerivation.Pbkdf2(
                password: "12345",
                salt: salt,
                prf: KeyDerivationPrf.HMACSHA256,
                iterationCount: 100000,
                numBytesRequested: 256 / 8));

            var user = new
            {
                PasswordHash = passwordHash,
                Salt = salt,
                Role = 0
            };
            var valueModel = new ValueModel(JsonSerializer.Serialize(user), new TimestampModel(1, Guid.Empty));
            yield return ("admin", valueModel);
        }
    }
}