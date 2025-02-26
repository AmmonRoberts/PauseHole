using API.Models;
using Refit;

namespace API.Services;

public interface IPiHoleCLient
{
	// Authenticate and get a session ID.
	[Post("/auth")]
	Task<AuthResponse> AuthenticateAsync([Body] AuthRequest request);

	[Get("/dns/blocking")]
	Task<string> GetStatusAsync([Header("Session-Id")] string sessionId);

	[Post("/dns/blocking")]
	Task<string> SetStatusAsync([Header("Session-Id")] string sessionId);
}