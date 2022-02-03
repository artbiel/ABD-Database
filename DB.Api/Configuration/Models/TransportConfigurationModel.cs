namespace ADBDB.Api.Configuration.Models
{
    public struct TransportConfigurationModel
    {
        public int? MaxRetryAttempts { get; set; }
        public int? InitialBackoff { get; set; }
        public int? MaxBackoff { get; set; }
        public int? BackoffMultiplier { get; set; }
    }
}
