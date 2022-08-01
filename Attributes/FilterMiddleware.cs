using Microsoft.Extensions.DependencyInjection;

namespace Attributes
{
    public static class ServiceExtension
    {
        public static void AddMyAttribute(this IServiceCollection Services)
        {
            Services.AddControllersWithViews(options =>
            {
                options.Filters.Add(new AttributeFilter());
            });
        }
    }
}