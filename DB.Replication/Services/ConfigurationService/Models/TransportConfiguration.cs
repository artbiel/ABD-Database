namespace ABDDB.Replication.Services
{
    public class TransportConfiguration
    {
        public TransportConfiguration(int? maxRetryAttempts,
            int? initialBackoff,
            int? maxBackoff,
            double? backoffMultiplier)
        {
            if (maxRetryAttempts is not null)
                MaxRetryAttempts = maxRetryAttempts.Value;
            if (initialBackoff is not null)
                InitialBackoff = TimeSpan.FromSeconds(initialBackoff.Value);
            if (maxBackoff is not null)
                MaxBackoff = TimeSpan.FromSeconds(maxBackoff.Value);
            if (backoffMultiplier is not null)
                BackoffMultiplier = backoffMultiplier.Value;

            if (MaxRetryAttempts < 2)
                throw new ArgumentOutOfRangeException(nameof(maxRetryAttempts));
            if (BackoffMultiplier <= 0)
                throw new ArgumentOutOfRangeException(nameof(backoffMultiplier));
            if (InitialBackoff > MaxBackoff)
                throw new ArgumentOutOfRangeException(nameof(maxBackoff));
        }

        public int MaxRetryAttempts { get; } = 3;
        public TimeSpan InitialBackoff { get; } = TimeSpan.FromSeconds(2);
        public TimeSpan MaxBackoff { get; } = TimeSpan.FromSeconds(8);
        public double BackoffMultiplier { get; } = 2;
    }
}