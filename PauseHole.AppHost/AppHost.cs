var builder = DistributedApplication.CreateBuilder(args);

var backend = builder.AddDotnetProject("API", "../API/API.csproj");

builder.AddJavaScriptApp("app", "../app", "start")
	.WithBun()
	.WithReference(backend)
	.WithHttpEndpoint(env: "PORT")
	.WithExternalHttpEndpoints()
	//.PublishAsDockerFile()
	.WaitFor(backend);

builder.Build().Run();