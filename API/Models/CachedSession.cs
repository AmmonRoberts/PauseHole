using System.Text.Json.Serialization;

namespace API.Models;

public class CachedSession
{
	public string SessionId { get; set; }

	public DateTime Expiry { get; set; }
}