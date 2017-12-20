using System;
using System.Collections.Generic;
using System.Diagnostics;
using MarvelApiMvcExample.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarvelApiMvcExample.Controllers
{
	public class HomeController : Controller
	{

		private readonly IConcreteApiMarvel _concreteApiMarvel;

		public HomeController(IConcreteApiMarvel concreteApiMarvel)
		{
			_concreteApiMarvel = concreteApiMarvel;
		}

		public IActionResult Index(int pagina)
		{
			if (pagina.Equals(null))
				pagina = 0;

			var resultado = _concreteApiMarvel.ChamadaApiMarvel(pagina);

			var personagens = new List<Personagem>();
			var totalItens = resultado.data.total;
			int totalpaginas = totalItens / 20;

			for (var i = 0; i < resultado.data.results.Count; i++)
				personagens.Add(new Personagem
				{
					Nome = resultado.data.results[i].name,
					Descricao = resultado.data.results[i].description,
					UrlImagem = resultado.data.results[i].thumbnail.path + "/portrait_incredible" + "." +
								resultado.data.results[i].thumbnail.extension
				});

			ViewBag.totalpaginas = Convert.ToInt32(totalpaginas);

			ViewBag.ListaPersonagens = personagens;
			return View();
		}
		

		public IActionResult DetalhesPersonagem(string name)
		{
			Personagem personagem;
			var resultado = _concreteApiMarvel.ChamadaApiMarvel(name);

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
			return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
		}
	}
}