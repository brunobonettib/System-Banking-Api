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
}