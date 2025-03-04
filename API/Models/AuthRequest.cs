using System.Text.Json.Serialization;

namespace API.Models;

public class AuthRequest
{
	[JsonPropertyName("password")]
	public string Password { get; set; }
}