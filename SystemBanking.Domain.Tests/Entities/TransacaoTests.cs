using SystemBanking.Domain.Entities;
using SystemBanking.Domain.Enums;

namespace SystemBanking.Domain.Tests.Entities;

public class TransacaoTests
{
    [Fact]
    public void DeveCriarTransacaoQuandoDadosForemValidos()
    {
        var contaBancariaId = Guid.NewGuid();
        var instanteAnterior = DateTime.UtcNow;

        var transacao = new Transacao(
            contaBancariaId,
            100m,
            TipoTransacao.Deposito,
            "Depósito realizado");

        var instantePosterior = DateTime.UtcNow;

        Assert.NotEqual(Guid.Empty, transacao.Id);
        Assert.Equal(contaBancariaId, transacao.ContaBancariaId);
        Assert.Equal(100m, transacao.Valor);
        Assert.Equal(TipoTransacao.Deposito, transacao.Tipo);
        Assert.Equal("Depósito realizado", transacao.Descricao);

        Assert.InRange(
            transacao.Data,
            instanteAnterior,
            instantePosterior);
    }
}