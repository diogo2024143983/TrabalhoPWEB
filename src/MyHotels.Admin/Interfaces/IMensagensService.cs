namespace MyHotels.Admin.Interfaces;

public record Mensagem(string Texto, bool Sucesso);

public interface IMensagensService
{
    void Sucesso(string texto);
    void Erro(string texto);

    Mensagem? Retira();
}