using System.Security.Cryptography;
using System.Text.Json;
using System.Text.Json.Serialization;
using IdentityModel;
using Microsoft.IdentityModel.Tokens;
// ReSharper disable InconsistentNaming - Justification: matching json/jwk standards.
#pragma warning disable CS8618 //- nullable nagging disabled here - Justification: default json/jwk contract.

namespace private_key_jwt_tool;

public static class JsonWebKeyExtensions
{
	/// <summary>
	/// Generate json containing the private key of a given <see cref="JsonWebKey"/>.
	/// The result can be used as a <see cref="JsonWebKey"/> ctor parameter to sign with.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GeneratePrivateKeyJson(this JsonWebKey key) => CryptoHelper.GeneratePrivateKeyJson(key);

	/// <summary>
	/// Generate json containing the public key of a given <see cref="JsonWebKey"/>.
	/// The result can be used as a <see cref="JsonWebKey"/> ctor parameter to validate signatures.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GeneratePublicKeyJson(this JsonWebKey key) => CryptoHelper.GeneratePublicKeyJson(key);
}

public static class CryptoHelper
{
	internal class PublicJsonWebKeyJson
	{
		public string kty { get; set; }
		public string use { get; set; }
		public string kid { get; set; }
		public string x5t { get; set; }
		public string e { get; set; }
		public string n { get; set; }
		public string[] x5c { get; set; }
		public string alg { get; set; }
		public string x { get; set; }
		public string y { get; set; }
		public string crv { get; set; }
	}

	internal class PrivateJsonWebKeyJson
	{
		public string kty { get; set; }
		public string use { get; set; }
		public string kid { get; set; }
		public string x5t { get; set; }
		public string e { get; set; }
		public string n { get; set; }
		public string[] x5c { get; set; }
		public string alg { get; set; }
		public string x { get; set; }
		public string y { get; set; }
		public string crv { get; set; }

		public string d { get; set; }
		public string dp { get; set; }
		public string dq { get; set; }
		public string p { get; set; }
		public string q { get; set; }
		public string qi { get; set; }
	}

	/// <summary>
	/// Generate json containing the private key of a given <see cref="JsonWebKey"/>.
	/// The result can be used as a <see cref="JsonWebKey"/> ctor parameter to sign with.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GeneratePrivateKeyJson(JsonWebKey key)
	{
		var webKey = new PrivateJsonWebKeyJson
		{
			kty = JsonWebAlgorithmsKeyTypes.RSA,
			use = key.Use ?? "sig",
			kid = key.KeyId,
			e = key.E,
			n = key.N,
			d = key.D,
			dp = key.DP,
			dq = key.DQ,
			p = key.P,
			q = key.Q,
			qi = key.QI
		};

		return JsonSerializer.Serialize(webKey,
			new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
	}

	/// <summary>
	/// Generate json containing the public key of a given <see cref="JsonWebKey"/>.
	/// The result can be used as a <see cref="JsonWebKey"/> ctor parameter to validate signatures.
	/// </summary>
	/// <param name="key"></param>
	/// <returns></returns>
	public static string GeneratePublicKeyJson(JsonWebKey key)
	{
		var webKey = new PublicJsonWebKeyJson
		{
			kty = JsonWebAlgorithmsKeyTypes.RSA,
			use = key.Use ?? "sig",
			kid = key.KeyId,
			e = key.E,
			n = key.N
		};

		return JsonSerializer.Serialize(webKey,
			new JsonSerializerOptions { DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull });
	}

	/// <summary>
	/// Generates a new <see cref="JsonWebKey"/> around an <see cref="RSA"/> private/public key pair
	/// and assigns it an kid.
	/// </summary>
	/// <param name="keySize"></param>
	/// <returns></returns>
	/// <exception cref="InvalidOperationException"></exception>
	public static JsonWebKey GenerateJsonWebKey(int keySize = 2048)
	{
		var key = CryptoHelper.CreateRsaSecurityKey(keySize);
		if (key.PrivateKeyStatus != PrivateKeyStatus.Exists)
		{
			throw new InvalidOperationException(
				"Somehow an RsaSecurity key without private key information was generated.");
		}

		var jwk = JsonWebKeyConverter.ConvertFromRSASecurityKey(key);
		return jwk;
	}

	/// <summary>
	/// Creates a new RSA security key.
	/// </summary>
	/// <returns></returns>
	private static RsaSecurityKey CreateRsaSecurityKey(int keySize = 2048)
	{
		//method from Duende.IdentityServer.Configuration.CryptoHelper
		return new RsaSecurityKey(RSA.Create(keySize))
		{
			KeyId = CryptoRandom.CreateUniqueId(16, CryptoRandom.OutputFormat.Hex)
		};
	}
	
}
