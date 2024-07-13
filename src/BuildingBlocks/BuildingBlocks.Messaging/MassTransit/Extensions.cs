using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace BuildingBlocks.Messaging.MassTransit;
public static class Extensions
{
    public static IServiceCollection AddMessageBroker(this IServiceCollection services, IConfiguration configuration, Assembly? assembly = null)
    {
        services.AddMassTransit(cfg =>
        {
            cfg.SetKebabCaseEndpointNameFormatter();

            if (assembly != null) 
                cfg.AddConsumers(assembly);

            cfg.UsingRabbitMq((context, configurator) =>
            {
                configurator.Host(
                        new Uri(configuration["MessageBroker:Host"]!), host =>
                        {
                            host.Username(configuration["MessageBroker:UserName"]!);
                            host.Password(configuration["MessageBroker:Password"]!);
                        });
                configurator.ConfigureEndpoints(context);
            });

        });

        return services;
    }
}
