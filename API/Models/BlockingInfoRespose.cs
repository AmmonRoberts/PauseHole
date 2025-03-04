using System.Text.Json.Serialization;

namespace API.Models;

public class BlockingInfoResponse
{
	[JsonPropertyName("blocking")]
	public string BlockingStatus { get; set; }

	[JsonPropertyName("timer")]
	public double? Timer { get; set; }
}