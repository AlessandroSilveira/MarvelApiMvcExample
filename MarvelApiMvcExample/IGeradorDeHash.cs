namespace MarvelApiMvcExample
{
	public interface IGeradorDeHash
	{string GerarHash(string ts, string publicKey, string privateKey);
	}
}