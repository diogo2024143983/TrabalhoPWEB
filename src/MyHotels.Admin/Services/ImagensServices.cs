using MyHotels.Admin.Interfaces;

namespace MyHotels.Admin.Services
{
    public class ImagensServices : IImagensService
    {

        public long TamanhoMax => 512_000;

        private static readonly string[] Extensoes = [".png", ".jpg", ".jpeg"];


        private const string SemImagem =
        "data:image/svg+xml;utf8,<svg xmlns='http://www.w3.org/2000/svg' width='80' height='60' viewBox='0 0 80 60'><rect width='80' height='60' fill='%23e9ecef'/><text x='50%25' y='52%25' dominant-baseline='middle' text-anchor='middle' fill='%236c757d' font-family='sans-serif' font-size='10'>Sem foto</text></svg>";


        public string? Valida(string nomeFicheiro, long tamanho)
        {
            var extensao = Path.GetExtension(nomeFicheiro).ToLowerInvariant();
            if (!Extensoes.Contains(extensao))
            {
                return "Só são aceites imagens .png, .jpg ou .jpeg.";
            }
            if (tamanho > TamanhoMax)
            {
                return $"A imagem tem {tamanho / 1024} KB. O máximo são {TamanhoMax / 1024} KB.";
            }
            return null;
        }
        public async Task<byte[]> LerBytesAsync(Stream conteudo)
        {
            using var memoria = new MemoryStream();
            await conteudo.CopyToAsync(memoria);
            return memoria.ToArray();
        }
        public string Src(byte[]? imagem, string? nomeFicheiro)
        {
            if (imagem is null || imagem.Length == 0)
            {
                return SemImagem;
            }
            var tipo = Path.GetExtension(nomeFicheiro)?.ToLowerInvariant() == ".png"
                ? "image/png"
                : "image/jpeg";
            return $"data:{tipo};base64,{Convert.ToBase64String(imagem)}";
        }
    }


}

