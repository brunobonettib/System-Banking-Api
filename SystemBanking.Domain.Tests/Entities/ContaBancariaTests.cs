using SystemBanking.Domain.Entities;
using SystemBanking.Domain.Enums;

namespace SystemBanking.Domain.Tests.Entities;

public class ContaBancariaTests
{
    [Fact]
    public void DeveDepositarValorQuandoValorForValido()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var conta = new ContaBancaria(
            "0001",
            cliente);

        conta.Depositar(100m);

        Assert.Equal(100m, conta.Saldo);
        Assert.Single(conta.Transacoes);

        var transacao = conta.Transacoes.First();

        Assert.Equal(100m, transacao.Valor);
        Assert.Equal(TipoTransacao.Deposito, transacao.Tipo);
        Assert.Equal("Depósito realizado", transacao.Descricao);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoDepositoForMenorOuIgualAZero()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var conta = new ContaBancaria(
            "0001",
            cliente);

        var excecao = Assert.Throws<ArgumentException>(() =>
            conta.Depositar(0m));

        Assert.Contains(
            "O valor do depósito deve ser maior que zero.",
            excecao.Message);

        Assert.Equal(0m, conta.Saldo);
        Assert.Empty(conta.Transacoes);
    }

    [Fact]
    public void DeveSacarValorQuandoSaldoForSuficiente()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var conta = new ContaBancaria(
            "0001",
            cliente);

        conta.Depositar(200m);
        conta.Sacar(80m);

        Assert.Equal(120m, conta.Saldo);
        Assert.Equal(2, conta.Transacoes.Count);

        var transacao = conta.Transacoes.Last();

        Assert.Equal(80m, transacao.Valor);
        Assert.Equal(TipoTransacao.Saque, transacao.Tipo);
        Assert.Equal("Saque realizado", transacao.Descricao);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoSaldoForInsuficiente()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var conta = new ContaBancaria(
            "0001",
            cliente);

        conta.Depositar(100m);

        var excecao = Assert.Throws<InvalidOperationException>(() =>
            conta.Sacar(150m));

        Assert.Contains(
            "Saldo insuficiente para realizar o saque.",
            excecao.Message);

        Assert.Equal(100m, conta.Saldo);
        Assert.Single(conta.Transacoes);
    }

    [Fact]
    public void DeveTransferirValorEntreContasQuandoDadosForemValidos()
    {
        var clienteOrigem = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var clienteDestino = new Cliente(
            "Maria Souza",
            "98765432100");

        var contaOrigem = new ContaBancaria(
            "0001",
            clienteOrigem);

        var contaDestino = new ContaBancaria(
            "0002",
            clienteDestino);

        contaOrigem.Depositar(300m);

        contaOrigem.Transferir(
            120m,
            contaDestino);

        Assert.Equal(180m, contaOrigem.Saldo);
        Assert.Equal(120m, contaDestino.Saldo);

        Assert.Equal(2, contaOrigem.Transacoes.Count);
        Assert.Single(contaDestino.Transacoes);

        var transacaoOrigem = contaOrigem.Transacoes.Last();
        var transacaoDestino = contaDestino.Transacoes.Single();

        Assert.Equal(
            TipoTransacao.Transferencia,
            transacaoOrigem.Tipo);

        Assert.Equal(
            TipoTransacao.Transferencia,
            transacaoDestino.Tipo);

        Assert.Equal(
            120m,
            transacaoOrigem.Valor);

        Assert.Equal(
            120m,
            transacaoDestino.Valor);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoContaDestinoForNula()
    {
        var cliente = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var contaOrigem = new ContaBancaria(
            "0001",
            cliente);

        contaOrigem.Depositar(200m);

        var excecao = Assert.Throws<ArgumentNullException>(() =>
            contaOrigem.Transferir(100m, null!));

        Assert.Equal("destino", excecao.ParamName);
        Assert.Equal(200m, contaOrigem.Saldo);
        Assert.Single(contaOrigem.Transacoes);
    }

    [Fact]
    public void DeveLancarExcecaoQuandoValorDaTransferenciaForMenorOuIgualAZero()
    {
        var clienteOrigem = new Cliente(
            "Bruno Bonetti",
            "12345678900");

        var clienteDestino = new Cliente(
            "Maria Souza",
            "98765432100");

        var contaOrigem = new ContaBancaria(
            "0001",
            clienteOrigem);

        var contaDestino = new ContaBancaria(
            "0002",
            clienteDestino);

        contaOrigem.Depositar(200m);

        var excecao = Assert.Throws<ArgumentException>(() =>
            contaOrigem.Transferir(0m, contaDestino));

        Assert.Contains(
            "O valor da transferência deve ser maior que zero.",
            excecao.Message);

        Assert.Equal(200m, contaOrigem.Saldo);
        Assert.Equal(0m, contaDestino.Saldo);

        Assert.Single(contaOrigem.Transacoes);
        Assert.Empty(contaDestino.Transacoes);
    }

    [Fact]
public void DeveLancarExcecaoQuandoValorDoSaqueForMenorOuIgualAZero()
{
    var cliente = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var conta = new ContaBancaria(
        "0001",
        cliente);

    conta.Depositar(200m);

    var excecao = Assert.Throws<ArgumentException>(() =>
        conta.Sacar(0m));

    Assert.Contains(
        "O valor do saque deve ser maior que zero.",
        excecao.Message);

    Assert.Equal(200m, conta.Saldo);
    Assert.Single(conta.Transacoes);
}

[Fact]
public void DeveLancarExcecaoQuandoSaldoForInsuficienteParaTransferencia()
{
    var clienteOrigem = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var clienteDestino = new Cliente(
        "Maria Souza",
        "98765432100");

    var contaOrigem = new ContaBancaria(
        "0001",
        clienteOrigem);

    var contaDestino = new ContaBancaria(
        "0002",
        clienteDestino);

    contaOrigem.Depositar(100m);

    var excecao = Assert.Throws<InvalidOperationException>(() =>
        contaOrigem.Transferir(150m, contaDestino));

    Assert.Contains(
        "Saldo insuficiente para realizar a transferência.",
        excecao.Message);

    Assert.Equal(100m, contaOrigem.Saldo);
    Assert.Equal(0m, contaDestino.Saldo);

    Assert.Single(contaOrigem.Transacoes);
    Assert.Empty(contaDestino.Transacoes);
}

[Fact]
public void DeveRegistrarDescricaoDaTransferenciaEnviada()
{
    var clienteOrigem = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var clienteDestino = new Cliente(
        "Maria Souza",
        "98765432100");

    var contaOrigem = new ContaBancaria(
        "0001",
        clienteOrigem);

    var contaDestino = new ContaBancaria(
        "0002",
        clienteDestino);

    contaOrigem.Depositar(300m);

    contaOrigem.Transferir(
        100m,
        contaDestino);

    var transferenciaEnviada =
        contaOrigem.Transacoes.Last();

    Assert.Equal(
        "Transferência enviada para a conta 0002",
        transferenciaEnviada.Descricao);
}

[Fact]
public void DeveRegistrarDescricaoDaTransferenciaRecebida()
{
    var clienteOrigem = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var clienteDestino = new Cliente(
        "Maria Souza",
        "98765432100");

    var contaOrigem = new ContaBancaria(
        "0001",
        clienteOrigem);

    var contaDestino = new ContaBancaria(
        "0002",
        clienteDestino);

    contaOrigem.Depositar(300m);

    contaOrigem.Transferir(
        100m,
        contaDestino);

    var transferenciaRecebida =
        contaDestino.Transacoes.Single();

    Assert.Equal(
        "Transferência recebida da conta 0001",
        transferenciaRecebida.Descricao);
}

[Fact]
public void DeveLancarExcecaoQuandoNumeroDaContaForVazio()
{
    var cliente = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var excecao = Assert.Throws<ArgumentException>(() =>
        new ContaBancaria("", cliente));

    Assert.Contains(
        "Número da conta é obrigatório.",
        excecao.Message);
}

[Fact]
public void DeveLancarExcecaoQuandoClienteForNulo()
{
    var excecao = Assert.Throws<ArgumentNullException>(() =>
        new ContaBancaria("0001", null!));

    Assert.Equal(
        "cliente",
        excecao.ParamName);
}

[Fact]
public void DeveCriarContaComSaldoZeroESemTransacoes()
{
    var cliente = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var conta = new ContaBancaria(
        "0001",
        cliente);

    Assert.NotEqual(Guid.Empty, conta.Id);
    Assert.Equal("0001", conta.NumeroConta);
    Assert.Equal(0m, conta.Saldo);
    Assert.Equal(cliente.Id, conta.ClienteId);
    Assert.Same(cliente, conta.Cliente);
    Assert.Empty(conta.Transacoes);
}

[Fact]
public void DeveLancarExcecaoQuandoDepositoForNegativo()
{
    var cliente = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var conta = new ContaBancaria(
        "0001",
        cliente);

    Assert.Throws<ArgumentException>(() =>
        conta.Depositar(-100m));

    Assert.Equal(0m, conta.Saldo);
    Assert.Empty(conta.Transacoes);
}

[Fact]
public void DeveLancarExcecaoQuandoTransferenciaForParaMesmaConta()
{
    var cliente = new Cliente(
        "Bruno Bonetti",
        "12345678900");

    var conta = new ContaBancaria(
        "0001",
        cliente);

    conta.Depositar(200m);

    var excecao = Assert.Throws<InvalidOperationException>(() =>
        conta.Transferir(100m, conta));

    Assert.Contains(
        "A conta de destino deve ser diferente da conta de origem.",
        excecao.Message);

    Assert.Equal(200m, conta.Saldo);
    Assert.Single(conta.Transacoes);
}
}