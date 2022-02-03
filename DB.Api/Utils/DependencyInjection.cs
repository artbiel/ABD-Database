using ABDDB.Api.RequestCoordinator;
using ABDDB.Api.Services;
using ABDDB.Replication.Actors;
using ABDDB.Replication.Contracts;
using ABDDB.Replication.Services;
using ABDDB.Replication.Transport;
using ADBDB.Api.Configuration.Models;
using Microsoft.AspNetCore.Authentication.Certificate;
using Microsoft.AspNetCore.Authentication.Cookies;

namespace ABDDB.Api.Utils
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddReplication
            (this IServiceCollection services, ConfigurationModel configuration) => services
                .AddScoped<ICoordinator, Coordinator>()
                .AddSingleton<IReplica, Replica>()
                .AddSingleton<IReplicasService, ReplicasService>()
                .AddSingleton<IConfigurationService, ConfigurationService>(s =>
                    new ConfigurationService(configuration.ClusterConfiguration, configuration.TransportConfiguration, configuration.SecurityConfiguration))
                .AddSingleton<IChannelPool, ChannelPool>((s) => new ChannelPool(
                    s.GetRequiredService<IConfigurationService>(),
                    services.BuildServiceProvider()));

        public static IServiceCollection AddApi(this IServiceCollection services) =>
            services.AddScoped<IRequestCoordinator, RequestCoordinator.RequestCoordinator>()
                .AddScoped<IAuthenticationService, AuthenticationService>()
                .AddHttpContextAccessor()
                .AddCustomAuthentication();

        private static IServiceCollection AddCustomAuthentication(this IServiceCollection services)
        {
            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie();

            services.AddAuthentication(
                CertificateAuthenticationDefaults.AuthenticationScheme)
                .AddCertificate(options =>
                {
                    options.AllowedCertificateTypes = CertificateTypes.All;
                    options.Events = new CertificateAuthenticationEvents
                    {
                        OnCertificateValidated = context =>
                        {
                            var config = context.HttpContext.RequestServices.GetRequiredService<IConfigurationService>();
                            var allowedThumbprints = config.SecurityConfiguration.AllowedThumbprints;
                            if (allowedThumbprints.Contains(context.ClientCertificate.Thumbprint))
                            {
                                context.Success();
                            }
                            return Task.CompletedTask;
                        },
                        OnAuthenticationFailed = context =>
                        {
                            return Task.CompletedTask;
                        }
                    };
                })
                .AddCertificateCache();

            services.AddAuthorization();

            return services;
        }
    }
}
