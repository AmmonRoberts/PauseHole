using System.Text.Json.Serialization;

namespace API.Models;

public class Session
{
	[JsonPropertyName("valid")]
	public bool Valid { get; set; }

	[JsonPropertyName("totp")]
	public bool TOTP { get; set; }

	[JsonPropertyName("sid")]
	public string SessionId { get; set; }

	[JsonPropertyName("csrf")]
	public string CSRFToken { get; set; }

	[JsonPropertyName("validity")]
	public int Validity { get; set; }

	[JsonPropertyName("message")]
	public string Message { get; set; }
}