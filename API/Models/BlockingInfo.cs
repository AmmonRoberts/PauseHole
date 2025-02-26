using System.Text.Json.Serialization;

namespace API.Models;

public class BlockingInfo
{
	[JsonPropertyName("blocking")]
	public BlockingStatus BlockingStatus { get; set; }

	[JsonPropertyName("timer")]
	public double Timer { get; set; }
}