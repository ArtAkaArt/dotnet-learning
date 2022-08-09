using Microsoft.Extensions.DependencyInjection;

namespace CustomAttributes
{
    public static class ServiceExtension
    {
        public static void AddMyAttribute(this IServiceCollection Services)
        {
            Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AlternativeCustomFilter());
            });
        }
    }
}