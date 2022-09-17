using Microsoft.Extensions.DependencyInjection;
using Npgsql;

namespace MyORM
{
    public static class ServiceExtension
    {
        public static void AddSuperDbContext<T>(this IServiceCollection services, string connection)  
                                                where T :SuperDbContext
        {
            var sd = (T)Activator.CreateInstance(typeof(T), connection)!;
            services.AddSingleton<T>(sd);
        }
    }
}
