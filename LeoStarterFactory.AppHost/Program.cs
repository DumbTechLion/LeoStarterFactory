using LeoStarterFactory.AppHost;

var builder = DistributedApplication.CreateBuilder(args);

var cache = builder.AddRedis(ServiceNames.Cache);

var messaging = builder.AddRabbitMQ(ServiceNames.Messaging);

var pqsqlDb = builder
    .AddPostgres(ServiceNames.Postgres)
    .AddDatabase(ServiceNames.PostgresDatabase);

var apiService = builder.AddProject<Projects.LeoStarterFactory_ApiService>(ServiceNames.ApiService)
    .WithExternalHttpEndpoints()
    .WithReference(pqsqlDb)
    .WithReference(cache)
    .WithReference(messaging);

builder.AddNpmApp(ServiceNames.Vue, "../LeoStarterFactory.Vue")
    .WithExternalHttpEndpoints()
    .WithHttpEndpoint(env: "PORT")
    .WithReference(apiService)
    .PublishAsDockerFile();

builder.Build().Run();
