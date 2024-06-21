using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

var builder = WebApplication.CreateBuilder(args);

builder.Services.Configure<PiHoleSettings>(builder.Configuration.GetSection("PiHoleSettings"));

builder.Services.AddHttpClient();

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.MapGet("/status", async (HttpClient client, IOptions<PiHoleSettings> config, CancellationToken ct) =>
{
	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		var statusResponse = await client.GetAsync($"http://{piHole.Value.URL}/admin/api.php?status&auth={piHole.Value.APIKey}", ct);

		var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

		response.Add(new PiHoleStatus
		{
			Status = statusDictionary?["status"] ?? "error",
			Address = piHole.Value.URL ?? "error",
		});
	}

	return response;
});

app.MapPost("/pause", async ([FromQuery] uint minutes, IOptions<PiHoleSettings> config, HttpClient client, CancellationToken ct) =>
{
	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		var statusResponse = await client.GetAsync($"http://{piHole.Value.URL}/admin/api.php?disable={minutes * 60}&auth={piHole.Value.APIKey}", ct);

		var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

		response.Add(new PiHoleStatus
		{
			Status = statusDictionary?["status"] ?? "error",
			Address = piHole.Value.URL ?? "error",
		});
	}

	return response;
});

app.MapPost("/unpause", async (IOptions<PiHoleSettings> config, HttpClient client, CancellationToken ct) =>
{
	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		var statusResponse = await client.GetAsync($"http://{piHole.Value.URL}/admin/api.php?enable&auth={piHole.Value.APIKey}", ct);

		var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

		response.Add(new PiHoleStatus
		{
			Status = statusDictionary?["status"] ?? "error",
			Address = piHole.Value.URL ?? "error",
		});
	}

	return response;
});


app.Run();