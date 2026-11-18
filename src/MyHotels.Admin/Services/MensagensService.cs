using MyHotels.Admin.Interfaces;

namespace MyHotels.Admin.Services;

public class MensagensService : IMensagensService
{
    private Mensagem? _pendente;

    public void Sucesso(string texto) => _pendente = new Mensagem(texto, true);

    public void Erro(string texto) => _pendente = new Mensagem(texto, false);

    public Mensagem? Retira()
    {
        var atual = _pendente;
        _pendente = null;
        return atual;
    }
}