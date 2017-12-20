using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MarvelApiMvcExample
{
	public class ConcreteApiMarvel : AbstractApiMarvel , IConcreteApiMarvel
	{
		private readonly IConfiguration _configuration;
		private readonly IGeradorDeHash _geradorDeHash;

		public ConcreteApiMarvel(IConfiguration configuration, IGeradorDeHash geradorDeHash)
		{
			_configuration = configuration;
			_geradorDeHash = geradorDeHash;
		}

		public override dynamic ChamadaApiMarvel(int pagina)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var ts = DateTime.Now.Ticks.ToString();
				var publicKey = _configuration.GetSection("MarvelComicsAPI:PublicKey").Value;
				var hash = _geradorDeHash.GerarHash(ts, publicKey,
					_configuration.GetSection("MarvelComicsAPI:PrivateKey").Value);

				var response = client.GetAsync(_configuration.GetSection("MarvelComicsAPI:BaseUrl").Value +
				                               $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
				                               $"offset={pagina * 20}&limit=20").Result;
				response.EnsureSuccessStatusCode();
				var conteudo = response.Content.ReadAsStringAsync().Result;

				dynamic resultado = JsonConvert.DeserializeObject(conteudo);
				return resultado;
			}
		}

		public override dynamic ChamadaApiMarvel(string name)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var ts = DateTime.Now.Ticks.ToString();
				var publicKey = _configuration.GetSection("MarvelComicsAPI:PublicKey").Value;
				var hash = _geradorDeHash.GerarHash(ts, publicKey,
					_configuration.GetSection("MarvelComicsAPI:PrivateKey").Value);

				var response = client.GetAsync(_configuration.GetSection("MarvelComicsAPI:BaseUrl").Value +
				                               $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
				                               $"name={name}").Result;
				response.EnsureSuccessStatusCode();
				var conteudo = response.Content.ReadAsStringAsync().Result;

				dynamic resultado = JsonConvert.DeserializeObject(conteudo);

				return resultado;
			}
		}
	}
}