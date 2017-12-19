using System;
using System.Security.Cryptography;
using System.Text;

namespace MarvelApiMvcExample
{
	public class GeradorDeHash
	{
		public static string GerarHash(string ts, string publicKey, string privateKey)
		{
			var bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
			var gerador = MD5.Create();
			var bytesHash = gerador.ComputeHash(bytes);
			return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
		}
	}
}