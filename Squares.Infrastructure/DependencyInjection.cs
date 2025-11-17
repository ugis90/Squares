using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Squares.Domain.Abstractions;
using Squares.Infrastructure.Persistence;
using Squares.Infrastructure.Repositories;
using Squares.Infrastructure.Services;

namespace Squares.Infrastructure;

public static class DependencyInjection
{
    private const string DefaultConnectionString = "Data Source=squares.db";
    private const string ConnectionStringName = "SquaresDb";

    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString(ConnectionStringName) ?? DefaultConnectionString;

        services.AddDbContext<SquaresDbContext>(options =>
        {
            options.UseSqlite(connectionString);
        });

        services.AddScoped<IPointListRepository, PointListRepository>();
        services.AddScoped<ISquareDetectionService, SquareDetectionService>();

        return services;
    }
}

