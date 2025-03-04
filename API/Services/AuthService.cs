using API.Models;
using Microsoft.Extensions.Caching.Memory;
using System.Text;
using System.Text.Json;

namespace API.Services;

public class AuthService
{
	private readonly ILogger<AuthService> _logger;
	private readonly IHttpClientFactory _factory;
	private readonly IMemoryCache _cache;

	public AuthService(ILogger<AuthService> logger, IHttpClientFactory factory, IMemoryCache cache)
	{
		_logger = logger;
		_factory = factory;
		_cache = cache;
	}

	public async Task<string> GetSessionAsync(KeyValuePair<string, PiHoleConfig> piHole, CancellationToken cancellationToken)
	{
		if (_cache.TryGetValue(piHole.Key, out CachedSession? session))
		{
			if (session?.Expiry < DateTime.UtcNow)
			{
				_logger.LogInformation("Cached session ID expired.");
				_cache.Remove(piHole.Key);
			}
			else
			{
				_logger.LogInformation("Using cached session ID.");

				return session.SessionId;
			}
		}

		_logger.LogInformation("Fetching new session ID.");

		var client = _factory.CreateClient("pausehole-backend");

		var authrequest = new AuthRequest { Password = piHole.Value.APIKey };

		using var jsonContent = new StringContent(JsonSerializer.Serialize(authrequest), Encoding.UTF8, "application/json");

		var authResponse = await client.PostAsync($"{piHole.Value.URL}/api/auth", jsonContent, cancellationToken);

		var responseContent = await authResponse.Content.ReadFromJsonAsync<AuthResponse>(cancellationToken);

		ArgumentException.ThrowIfNullOrEmpty(responseContent?.Session?.SessionId, "Session ID missing in response.");

		var newSession = new CachedSession
		{
			SessionId = responseContent.Session.SessionId,
			Expiry = DateTime.UtcNow.AddSeconds((double)responseContent?.Session?.Validity)
		};

		_cache.Set(piHole.Key, newSession);

		return newSession.SessionId;
	}
}