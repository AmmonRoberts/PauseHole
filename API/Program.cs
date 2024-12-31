using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;

var builder = WebApplication.CreateBuilder(args);

builder.AddServiceDefaults();

builder.Services.Configure<PiHoleSettings>(builder.Configuration.GetSection("PiHoleSettings"));

builder.Services.AddHttpClient("pausehole-backend")
	.AddResilienceHandler("my-pipeline", builder =>
	{
		// Refer to https://www.pollydocs.org/strategies/retry.html#defaults for retry defaults
		builder.AddRetry(new HttpRetryStrategyOptions
		{
			MaxRetryAttempts = 1,
			Delay = TimeSpan.FromSeconds(2),
			BackoffType = DelayBackoffType.Constant
		});

		// Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
		builder.AddTimeout(TimeSpan.FromSeconds(3));
	});

builder.Services.AddCors();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.MapGet("/status", async (IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, CancellationToken ct) =>
{
	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var statusResponse = await client.GetAsync($"{piHole.Value.URL}/admin/api.php?status&auth={piHole.Value.APIKey}", ct);

			var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

			response.Add(new PiHoleStatus
			{
				Status = statusDictionary?["status"] ?? "error",
				Address = piHole.Value.URL ?? "error",
			});
		}
		catch (Exception e)
		{
			response.Add(new PiHoleStatus
			{
				Status = "error",
				Address = piHole.Value.URL ?? "error",
				ErrorMessage = e.Message
			});
		}
	}

	return response;
});

app.MapPost("/pause", async ([FromQuery] uint minutes, IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, CancellationToken ct) =>
{
	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var statusResponse = await client.GetAsync($"{piHole.Value.URL}/admin/api.php?disable={minutes * 60}&auth={piHole.Value.APIKey}", ct);

			var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

			response.Add(new PiHoleStatus
			{
				Status = statusDictionary?["status"] ?? "error",
				Address = piHole.Value.URL ?? "error",
			});
		}
		catch (Exception e)
		{
			response.Add(new PiHoleStatus
			{
				Status = "error",
				Address = piHole.Value.URL ?? "error",
				ErrorMessage = e.Message
			});
		}
	}

	return response;
});

app.MapPost("/unpause", async (IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, CancellationToken ct) =>
{
	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var statusResponse = await client.GetAsync($"{piHole.Value.URL}/admin/api.php?enable&auth={piHole.Value.APIKey}", ct);

			var statusDictionary = await statusResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>(ct);

			response.Add(new PiHoleStatus
			{
				Status = statusDictionary?["status"] ?? "error",
				Address = piHole.Value.URL ?? "error",
			});
		}
		catch (Exception e)
		{
			response.Add(new PiHoleStatus
			{
				Status = "error",
				Address = piHole.Value.URL ?? "error",
				ErrorMessage = e.Message
			});
		}
	}

	return response;
});

await app.RunAsync();