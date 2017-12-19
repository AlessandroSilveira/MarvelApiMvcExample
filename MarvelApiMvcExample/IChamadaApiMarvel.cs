namespace MarvelApiMvcExample
{
	public interface IChamadaApiMarvel
	{
		dynamic ChamadaApiMarvel(int? pagina);
		dynamic ChamadaApiMarvel(string name);
	}
}