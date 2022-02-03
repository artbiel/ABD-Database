using Microsoft.Extensions.DependencyInjection;

namespace ABDDB.LocalStorage
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddLocalStorage(this IServiceCollection services)
        {
            return services
                .AddSingleton<ILocalStorage, LocalStorage>();
        }
    }
}