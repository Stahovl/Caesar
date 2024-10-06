using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Caesar.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caesar.Infrastructure;

/// <summary>
/// Provides a static method for registering infrastructure services in the dependency injection container.
/// </summary>
public static class InfrastructureServiceRegistration
{
    /// <summary>
    /// Adds infrastructure services to the specified IServiceCollection.
    /// </summary>
    /// <param name="services">The IServiceCollection to add services to.</param>
    /// <param name="configuration">The configuration containing connection strings and other settings.</param>
    /// <returns>The IServiceCollection for chaining.</returns>
    /// <remarks>
    /// This method performs the following registrations:
    /// - Adds the CaesarDbContext using SQL Server with the connection string from configuration.
    /// - Registers IReservationRepository with its concrete implementation ReservationRepository.
    /// - Registers IMenuItemRepository with its concrete implementation MenuItemRepository.
    /// - Registers IOrderRepository with its concrete implementation OrderRepository.
    /// All repositories are registered with a scoped lifetime.
    /// </remarks>
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<CaesarDbContext>(options =>
            options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        services.AddScoped<IReservationRepository, ReservationRepository>();
        services.AddScoped<IMenuItemRepository, MenuItemRepository>(); 
        services.AddScoped<IOrderRepository, OrderRepository>();

        return services;
    }
}
