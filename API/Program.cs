using API.Models;
using API.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Http.Resilience;
using Microsoft.Extensions.Options;
using Polly;
using System.Text.Json;

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
			Delay = TimeSpan.FromSeconds(1),
			BackoffType = DelayBackoffType.Constant
		});

		// Refer to https://www.pollydocs.org/strategies/timeout.html#defaults for timeout defaults
		builder.AddTimeout(TimeSpan.FromSeconds(2));
	});

builder.Services.AddCors();

builder.Services.AddMemoryCache();
builder.Services.AddSingleton<AuthService>();

var app = builder.Build();

app.UseCors(x => x.AllowAnyHeader().AllowAnyMethod().WithOrigins("*"));

app.MapGet("/status", async (IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, AuthService authService, CancellationToken cancellationToken) =>
{

	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var session = await authService.GetSessionAsync(piHole, cancellationToken);

			var request = new HttpRequestMessage(HttpMethod.Get, $"{piHole.Value.URL}/api/dns/blocking");
			request.Headers.Add("sid", session);

			var statusResponse = await client.SendAsync(request, cancellationToken);

			var blockingStatus = await statusResponse.Content.ReadFromJsonAsync<BlockingInfoResponse>(cancellationToken);

			response.Add(new PiHoleStatus
			{
				Status = blockingStatus?.BlockingStatus ?? "error",
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

app.MapPost("/pause", async ([FromQuery] uint minutes, IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, AuthService authService, CancellationToken cancellationToken) =>
{
	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var session = await authService.GetSessionAsync(piHole, cancellationToken);

			var requestBody = new BlockingInfoRequest
			{
				BlockingStatus = false,
				Timer = minutes * 60
			};

			var request = new HttpRequestMessage(HttpMethod.Post, $"{piHole.Value.URL}/api/dns/blocking");
			request.Headers.Add("sid", session);
			request.Content = new StringContent(JsonSerializer.Serialize(requestBody));

			var statusResponse = await client.SendAsync(request, cancellationToken);

			var blockingStatus = await statusResponse.Content.ReadFromJsonAsync<BlockingInfoResponse>(cancellationToken);

			response.Add(new PiHoleStatus
			{
				Status = blockingStatus?.BlockingStatus ?? "error",
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

app.MapPost("/unpause", async (IHttpClientFactory factory, IOptionsSnapshot<PiHoleSettings> config, AuthService authService, CancellationToken cancellationToken) =>
{
	var client = factory.CreateClient("pausehole-backend");

	var response = new List<PiHoleStatus>();

	foreach (var piHole in config.Value.PiHoles.ToList())
	{
		try
		{
			var session = await authService.GetSessionAsync(piHole, cancellationToken);

			var requestBody = new BlockingInfoRequest
			{
				BlockingStatus = true
			};

			var request = new HttpRequestMessage(HttpMethod.Post, $"{piHole.Value.URL}/api/dns/blocking");
			request.Headers.Add("sid", session);
			request.Content = new StringContent(JsonSerializer.Serialize(requestBody));

			var statusResponse = await client.SendAsync(request, cancellationToken);

			var blockingStatus = await statusResponse.Content.ReadFromJsonAsync<BlockingInfoResponse>(cancellationToken);

			response.Add(new PiHoleStatus
			{
				Status = blockingStatus?.BlockingStatus ?? "error",
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
