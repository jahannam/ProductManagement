using ProductManagement.Infrastructure;

namespace ProductManagement.Migrations
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = Host.CreateApplicationBuilder(args);

            builder.AddServiceDefaults();

            builder.Services.AddHostedService<Worker>();

            builder.Services.AddOpenTelemetry()
                .WithTracing(tracing => tracing.AddSource(Worker.ActivitySourceName));
            builder.AddNpgsqlDbContext<ProductDbContext>("productdb");

            var host = builder.Build();
            host.Run();
        }
    }
}