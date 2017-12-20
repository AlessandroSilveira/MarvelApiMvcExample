namespace MarvelApiMvcExample
{
	public interface IConcreteApiMarvel
	{
		dynamic ChamadaApiMarvel(int pagina);
		dynamic ChamadaApiMarvel(string name);
	}
}