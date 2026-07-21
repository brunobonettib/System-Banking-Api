using SystemBanking.Domain.Entities;

namespace SystemBanking.Domain.Tests.Entities;

public class ClienteTests
{
    [Fact]
    public void DeveCriarClienteQuandoDadosForemValidos()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        Assert.NotEqual(Guid.Empty, cliente.Id);
        Assert.Equal("Bruno Bonetti", cliente.Nome);
        Assert.Equal("12345678900", cliente.Documento);
        Assert.Empty(cliente.Contas);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoNomeForVazio()
    {
        var excecao = Assert.Throws<ArgumentException>(() =>
            new Cliente("", "12345678900"));

        Assert.Contains(
            "O nome do cliente é obrigatório.",
            excecao.Message);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoDocumentoForVazio()
    {
        var excecao = Assert.Throws<ArgumentException>(() =>
            new Cliente("Bruno Bonetti", ""));

        Assert.Contains(
            "O documento do cliente é obrigatório.",
            excecao.Message);
    }

    [Fact]
    public void DeveAdicionarContaAoCliente()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var conta = new ContaBancaria(
            "0001",
            cliente);

        cliente.AdicionarConta(conta);

        Assert.Single(cliente.Contas);
        Assert.Contains(conta, cliente.Contas);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoContaDuplicadaForAdicionada()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var primeiraConta = new ContaBancaria(
            "0001",
            cliente);

        var segundaConta = new ContaBancaria(
            "0001",
            cliente);

        cliente.AdicionarConta(primeiraConta);

        var excecao = Assert.Throws<InvalidOperationException>(() =>
            cliente.AdicionarConta(segundaConta));

        Assert.Contains(
            "Já existe uma conta com esse número para o cliente.",
            excecao.Message);

        Assert.Single(cliente.Contas);
    }
}