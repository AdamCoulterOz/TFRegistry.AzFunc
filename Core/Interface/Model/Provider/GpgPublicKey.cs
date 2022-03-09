using System.Text.Json.Serialization;

namespace PurpleDepot.Core.Interface.Model.Provider;
public class GpgPublicKey
{
	[JsonPropertyName("key_id")]
	public string KeyId { get; set; }

	[JsonPropertyName("ascii_armor")]
	public string AsciiArmor { get; set; }

	[JsonPropertyName("trust_signature")]
	public string? TrustSignature { get; set; }

	[JsonPropertyName("source")]
	public string? Source { get; set; }

	[JsonPropertyName("source_url")]
	public Uri? SourceUrl { get; set; }

	[JsonConstructor]
	public GpgPublicKey(string key_id, string ascii_armor, string? trust_signature = null, string? source = null, Uri? source_url = null)
		=> (KeyId, AsciiArmor, TrustSignature, Source, SourceUrl) = (key_id, ascii_armor, trust_signature, source, source_url);

#nullable disable
	protected GpgPublicKey() { }
}
