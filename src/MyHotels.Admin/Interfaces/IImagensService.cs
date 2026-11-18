namespace MyHotels.Admin.Interfaces
{
    public interface IImagensService
    {

        long TamanhoMax { get; }

        string? Valida(String nomeFich, long tamanho);

        Task<byte[]> LerBytesAsync(Stream conteudo);

        string Src(byte[]? imagem, string? nomeFicheiro);

    }
}
