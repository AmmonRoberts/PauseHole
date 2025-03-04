using System.Text.Json.Serialization;

namespace API.Models;

public class BlockingInfoRequest
{
	[JsonPropertyName("blocking")]
	public bool BlockingStatus { get; set; }

	[JsonPropertyName("timer")]
	public double? Timer { get; set; }
}