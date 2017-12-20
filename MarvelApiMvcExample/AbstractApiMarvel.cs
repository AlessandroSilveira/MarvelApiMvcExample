namespace MarvelApiMvcExample
{
	public abstract class AbstractApiMarvel
	{
		public abstract dynamic ChamadaApiMarvel(int pagina);
		public abstract dynamic ChamadaApiMarvel(string name);

		public void TemplateMethod(int pagina, string name)
		{
			ChamadaApiMarvel(pagina);
			ChamadaApiMarvel(name);
		}
	}
}