var builder = DistributedApplication.CreateBuilder(args);
var postgres = builder.AddPostgres("postgres")
                        .AddDatabase("productdb");

builder.AddProject<Projects.ProductManagement_Migrations>("migrations")
    .WithReference(postgres)
    .WaitFor(postgres);

var api = builder.AddProject<Projects.ProductManagement_Api>("productmanagementapi")
    .WithReference(postgres)
    .WaitFor(postgres)
    .WithExternalHttpEndpoints();

builder.AddNpmApp("react", "../app")
    .WithReference(api)
    .WaitFor(api)
    .WithEnvironment("BROWSER", "none")
    .WithHttpEndpoint(env: "PORT")
    .WithExternalHttpEndpoints()
    .PublishAsDockerFile();

builder.Build().Run();
