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
        Assert.Equal(DateTimeKind.Utc, transacao.Data.Kind);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoContaBancariaIdForVazio()
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Transacao(Guid.Empty, 100m, TipoTransacao.Deposito, "Depósito"));

        Assert.Contains("conta bancária", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("contaBancariaId", exception.ParamName);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-50)]
    public void DeveLancarExcecaoQuandoValorForMenorOuIgualAZero(decimal valor)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Transacao(Guid.NewGuid(), valor, TipoTransacao.Saque, "Saque"));

        Assert.Contains("maior que zero", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("valor", exception.ParamName);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoTipoForInvalido()
    {
        var tipoInvalido = (TipoTransacao)999;

        var exception = Assert.Throws<ArgumentOutOfRangeException>(() =>
            new Transacao(Guid.NewGuid(), 100m, tipoInvalido, "Transação inválida"));

        Assert.Equal("tipo", exception.ParamName);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("   ")]
    public void DeveLancarExcecaoQuandoDescricaoForNulaOuVazia(string? descricao)
    {
        var exception = Assert.Throws<ArgumentException>(() =>
            new Transacao(Guid.NewGuid(), 100m, TipoTransacao.Deposito, descricao!));

        Assert.Contains("descrição", exception.Message, StringComparison.OrdinalIgnoreCase);
        Assert.Equal("descricao", exception.ParamName);
    }

    [Fact]
    public void DeveRemoverEspacosExtrasDaDescricao()
    {
        var transacao = new Transacao(
            Guid.NewGuid(),
            150m,
            TipoTransacao.Deposito,
            "  Depósito com espaço  ");

        Assert.Equal("Depósito com espaço", transacao.Descricao);
    }
}