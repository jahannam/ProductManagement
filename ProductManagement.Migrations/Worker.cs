using Microsoft.EntityFrameworkCore;
using OpenTelemetry.Trace;
using ProductManagement.Domain;
using ProductManagement.Infrastructure;
using System.Diagnostics;

namespace ProductManagement.Migrations
{
    public class Worker(
    IServiceProvider serviceProvider,
    IHostApplicationLifetime hostApplicationLifetime) : BackgroundService
    {
        public const string ActivitySourceName = "Migrations";
        private static readonly ActivitySource s_activitySource = new(ActivitySourceName);

        protected override async Task ExecuteAsync(CancellationToken cancellationToken)
        {
            using var activity = s_activitySource.StartActivity("Migrating database", ActivityKind.Client);

            try
            {
                using var scope = serviceProvider.CreateScope();
                var dbContext = scope.ServiceProvider.GetRequiredService<ProductDbContext>();

                await RunMigrationAsync(dbContext, cancellationToken);
                await SeedDataAsync(dbContext, cancellationToken);
            }
            catch (Exception ex)
            {
                activity?.RecordException(ex);
                throw;
            }

            hostApplicationLifetime.StopApplication();
        }

        private static async Task RunMigrationAsync(ProductDbContext dbContext, CancellationToken cancellationToken)
        {
            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Run migration in a transaction to avoid partial migration if it fails.
                await dbContext.Database.MigrateAsync(cancellationToken);
            });
        }

        private static async Task SeedDataAsync(ProductDbContext dbContext, CancellationToken cancellationToken)
        {
            var products = new List<Product>()
            {
                new () { ProductCode = "BT1", Name = "Big truck 1", Category = "Vehicle", 
                    Price = 170.5M, Quantity = 10, DateAdded = DateTimeOffset.UtcNow },
                new () { ProductCode = "BT2", Name = "Big truck 2", Category = "Vehicle", 
                    Price = 70.5M, Quantity = 31, DateAdded = DateTimeOffset.UtcNow.AddYears(-1) },
                new () { ProductCode = "BB", Name = "Big boat", Category = "Boat", 
                    Price = 211.5M, Quantity = 12, DateAdded = DateTimeOffset.UtcNow.AddMonths(-1) },
                new () { ProductCode = "SB", Name = "Small boat", Category = "Boat",
                    Price = 32.5M, Quantity = 42, DateAdded = DateTimeOffset.UtcNow.AddMonths(-2) },
                new () { ProductCode = "HC", Name = "Helicopter", Category = "Helicopter",
                    Price = 1332.5M, Quantity = 22, DateAdded = DateTimeOffset.UtcNow.AddMonths(-4) },
            };

            var strategy = dbContext.Database.CreateExecutionStrategy();
            await strategy.ExecuteAsync(async () =>
            {
                // Seed the database
                await using var transaction = await dbContext.Database.BeginTransactionAsync(cancellationToken);
                foreach (var product in products)
                {
                    await dbContext.Products.AddAsync(product, cancellationToken);
                }
                await dbContext.SaveChangesAsync(cancellationToken);
                await transaction.CommitAsync(cancellationToken);
            });

        }
    }
}
