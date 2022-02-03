using ADBDB.Api.Configuration.Models;

namespace ABDDB.Api.Configuration.Models
{
    public class SecurityConfigurationModel
    {
        public CertificateModel Certificate { get; set; }
        public string[] AllowedThumbprints { get; set; }
    }
}
