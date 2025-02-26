using API.Models;
using Microsoft.Extensions.Caching.Memory;

namespace API.Services;

public class AuthService
{
	private readonly ILogger<AuthService> _logger;
	private readonly IPiHoleCLient _piholeClient;
	private readonly IMemoryCache _cache;
	private const string CacheKey = "ExternalApiSession";
	private static readonly TimeSpan SessionExpiry = TimeSpan.FromMinutes(30);

	public AuthService(ILogger<AuthService> logger, IPiHoleCLient piholeClient, IMemoryCache cache)
	{
		_logger = logger;
		_piholeClient = piholeClient;
		_cache = cache;
	}

	public async Task<string> GetSessionAsync()
	{
		if (_cache.TryGetValue(CacheKey, out string? sessionId) && !string.IsNullOrEmpty(sessionId))
		{
			_logger.LogInformation("Using cached session ID.");

			return sessionId;
		}

		_logger.LogInformation("Fetching new session ID.");

		var request = new AuthRequest { Password = "password" };

		var response = await _piholeClient.AuthenticateAsync(request);

		sessionId = response?.Session?.SessionId;

		ArgumentException.ThrowIfNullOrEmpty(sessionId, "Session ID missing in response.");

		_cache.Set(CacheKey, sessionId, new MemoryCacheEntryOptions
		{
			AbsoluteExpirationRelativeToNow = SessionExpiry
		});

		return sessionId;
	}
}