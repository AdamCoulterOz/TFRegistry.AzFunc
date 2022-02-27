using System.Text.Json.Serialization;

namespace PurpleDepot.Interface.Model.Provider;
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
	public GpgPublicKey(string keyId, string asciiArmor, string? trustSignature = null, string? source = null, Uri? sourceUrl = null)
		=> (KeyId, AsciiArmor, TrustSignature, Source, SourceUrl) = (keyId, asciiArmor, trustSignature, source, sourceUrl);

}
