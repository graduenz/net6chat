using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Net6Chat.Domain;
using Net6Chat.Domain.Services;
using Net6Chat.Domain.Services.Impl;

namespace Net6Chat.IoC
{
    public static class DependencyInjectionBootstrapper
    {
        public static IServiceCollection RegisterApplicationDependencies(IServiceCollection services, IConfiguration configuration) =>
            services
                .AddEntityFramework(configuration)
                .AddCap(configuration)
                .AddDomainServices();

        public static IServiceCollection AddEntityFramework(this IServiceCollection services, IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("SqlContext");

            services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString, options => {
                    options.MigrationsAssembly("Net6Chat.Migrations");
                }));

            return services;
        }

        public static IServiceCollection AddCap(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddCap(x =>
            {
                x.UseEntityFramework<ApplicationDbContext>();
                x.UseRabbitMQ(o => {
                    o.ConnectionFactoryOptions = x => x.Uri = new Uri(configuration.GetConnectionString("RabbitMQ"));
                });
            });

            return services;
        }

        private static IServiceCollection AddDomainServices(this IServiceCollection services) =>
            services.AddScoped<IChatService, DefaultChatService>();
    }
}