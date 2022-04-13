using Microsoft.Extensions.DependencyInjection;
using WebApiMyLib.BLL.Interfaces;
using WebApiMyLib.BLL.Services;

namespace WebApiMyLib.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureLoggerService(this IServiceCollection services) =>
            services.AddScoped<ILoggerManager, LoggerManager>();
    }
}
