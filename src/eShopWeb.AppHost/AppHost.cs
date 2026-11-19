var builder = DistributedApplication.CreateBuilder(args);

var seq = builder.AddSeq("seq")
                 .ExcludeFromManifest()
                 .WithLifetime(ContainerLifetime.Persistent)
                 .WithEnvironment("ACCEPT_EULA", "Y");

builder
    .AddDotnetProject("publicapi", "../PublicApi/PublicApi.csproj")
        .WithReference(seq)
        .WaitFor(seq);

builder
    .AddDotnetProject("web", "../Web/Web.csproj")
        .WithReference(seq)
        .WaitFor(seq);

builder.Build().Run();
