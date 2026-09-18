using CompleteMe.Infrastructure.Data;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

namespace CompleteMe.Api.Tests;

public class CustomWebApplicationFactory : WebApplicationFactory<Program>
{
    protected override void ConfigureWebHost(IWebHostBuilder builder)
    {
        builder.UseEnvironment("Development");

        builder.ConfigureServices(services =>
        {
            services.RemoveAll(typeof(DbContextOptions<AppDbContext>));

            var connectionString =
                "Server=(localdb)\\MSSQLLocalDB;Database=CompleteMeApiTests;Trusted_Connection=True;MultipleActiveResultSets=true";

            services.AddDbContext<AppDbContext>(options =>
            {
                options.UseSqlServer(connectionString);
            });

            using var scope = services.BuildServiceProvider().CreateScope();
            var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();

            db.Database.EnsureDeleted();
            db.Database.EnsureCreated();
        });
    }
}