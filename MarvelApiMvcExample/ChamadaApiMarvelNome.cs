using System;
using System.Net.Http;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MarvelApiMvcExample
{
	public class ChamadaApiMarvelNome : IChamadaApiMarvel
	{
		private readonly IConfiguration config;

		public ChamadaApiMarvelNome(IConfiguration config)
		{
			this.config = config;
		}

		public dynamic ChamadaApiMarvel(int? pagina)
		{
			throw new NotImplementedException();
		}

		public dynamic ChamadaApiMarvel(string name)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var ts = DateTime.Now.Ticks.ToString();
				var publicKey = config.GetSection("MarvelComicsAPI:PublicKey").Value;
				var hash = GeradorDeHash.GerarHash(ts, publicKey,
					config.GetSection("MarvelComicsAPI:PrivateKey").Value);

				var response = client.GetAsync(config.GetSection("MarvelComicsAPI:BaseUrl").Value +
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