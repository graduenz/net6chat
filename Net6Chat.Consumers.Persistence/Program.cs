using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Net6Chat.Consumers.Persistence;
using Net6Chat.IoC;

await Host.CreateDefaultBuilder(args)
    .ConfigureAppConfiguration(cfg => cfg.AddJsonFile("hostsettings.json"))
    .ConfigureServices((ctx, services) =>
    {
        services.AddTransient<PersistenceSub>();
        DependencyInjectionBootstrapper.RegisterApplicationDependencies(services, ctx.Configuration);
    })
    .Build()
    .RunAsync();