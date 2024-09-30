using Caesar.Core.Interfaces;
using Caesar.Infrastructure.Data;
using Caesar.Infrastructure.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace Caesar.Infrastructure;

public static class InfrastructureServiceRegistration
{
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
