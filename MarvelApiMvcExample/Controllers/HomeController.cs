using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using MarvelApiMvcExample.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;

namespace MarvelApiMvcExample.Controllers
{
	public class HomeController : Controller
	{
	    private readonly IConfiguration config;

	    public HomeController(IConfiguration config)
	    {
	        this.config = config;
	    }

	    public IActionResult Index(int? pagina)
	    {
	        if (pagina.Equals(null))
	            pagina = 0;
            

			var resultado = ChamadaApiMarvel(pagina);

			var personagens = new List<Personagem>();
	        var totalItens = resultado.data.total;
	        int totalpaginas = totalItens / 20;


			for (var i = 0; i < resultado.data.results.Count; i++)
				personagens.Add(new Personagem
				{
					Nome = resultado.data.results[i].name,
					Descricao = resultado.data.results[i].description,
					UrlImagem = resultado.data.results[i].thumbnail.path + "/portrait_incredible"+ "." +
					            resultado.data.results[i].thumbnail.extension
				});

	        ViewBag.totalpaginas = Convert.ToInt32(totalpaginas);

            ViewBag.ListaPersonagens = personagens;
			return View();
		}

		private dynamic ChamadaApiMarvel(int? pagina)
		{
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var ts = DateTime.Now.Ticks.ToString();
				var publicKey = config.GetSection("MarvelComicsAPI:PublicKey").Value;
				var hash = GerarHash(ts, publicKey,
					config.GetSection("MarvelComicsAPI:PrivateKey").Value);

				var response = client.GetAsync(config.GetSection("MarvelComicsAPI:BaseUrl").Value +
				                               $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
				                               $"offset={pagina*20}&limit=20").Result;
				response.EnsureSuccessStatusCode();
				var conteudo = response.Content.ReadAsStringAsync().Result;

				dynamic resultado = JsonConvert.DeserializeObject(conteudo);
				return resultado;
			}
		}

		
		private string GerarHash(string ts, string publicKey, string privateKey)
		{
			var bytes = Encoding.UTF8.GetBytes(ts + privateKey + publicKey);
			var gerador = MD5.Create();
			var bytesHash = gerador.ComputeHash(bytes);
			return BitConverter.ToString(bytesHash).ToLower().Replace("-", string.Empty);
		}

		
		public IActionResult DetalhesPersonagem(string name )
		{
			Personagem personagem;
			using (var client = new HttpClient())
			{
				client.DefaultRequestHeaders.Accept.Clear();
				client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

				var ts = DateTime.Now.Ticks.ToString();
				var publicKey = config.GetSection("MarvelComicsAPI:PublicKey").Value;
				var hash = GerarHash(ts, publicKey,
					config.GetSection("MarvelComicsAPI:PrivateKey").Value);

				var response = client.GetAsync(config.GetSection("MarvelComicsAPI:BaseUrl").Value +
				                               $"characters?ts={ts}&apikey={publicKey}&hash={hash}&" +
				                               $"name={name}").Result;
				response.EnsureSuccessStatusCode();
				var conteudo = response.Content.ReadAsStringAsync().Result;

				dynamic resultado = JsonConvert.DeserializeObject(conteudo);

				if (resultado.data.results.Count > 0)
					personagem = new Personagem
					{
						Nome = resultado.data.results[0].name,
						Descricao = resultado.data.results[0].description,
						UrlImagem = resultado.data.results[0].thumbnail.path + "/portrait_incredible" + "." +
						            resultado.data.results[0].thumbnail.extension,
						UrlWiki = resultado.data.results[0].urls[1].url
					};
				else
					personagem = new Personagem
					{
						Nome = "",
						Descricao = "Personagem não encontrado",
						UrlImagem = "",
						UrlWiki = ""
					};
			}
			return View(personagem);
		}



		public IActionResult About()
		{
			ViewData["Message"] = "Your application description page.";

			return View();
		}

		public IActionResult Contact()
		{
			ViewData["Message"] = "Your contact page.";

			return View();
		}

		public IActionResult Error()
		{
			return View(new ErrorViewModel {RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier});
		}
	}
}