using System.Security.Cryptography.X509Certificates;

namespace ABDDB.Replication.Services
{
    public class SecurityConfiguration
    {
        public SecurityConfiguration(X509Certificate2 certificate, string[] allowedThumbprints)
        {
            Certificate = certificate ?? throw new ArgumentNullException(nameof(certificate));
            if (allowedThumbprints is null || allowedThumbprints.Length == 0)
                throw new ArgumentNullException(nameof(allowedThumbprints));
            AllowedThumbprints = allowedThumbprints;
        }
        public X509Certificate2 Certificate { get; }
        public string[] AllowedThumbprints { get; }
    }
}