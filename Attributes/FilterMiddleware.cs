using Microsoft.Extensions.DependencyInjection;

namespace CustomAttributes
{
    public static class ServiceExtension
    {
        public static void AddMyAttribute(this IServiceCollection services)
        {
            services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new CacheCustomFilter());
            });
        }
    }
}