using Moq;
using SystemBanking.Application.Abstractions.Persistence;
using SystemBanking.Application.Clientes.CadastrarCliente;
using SystemBanking.Domain.Entities;

namespace SystemBanking.Application.Tests.Clientes.CadastrarCliente;

public class CadastrarClienteHandlerTests
{
    [Fact]
    public async Task DeveCadastrarClienteQuandoDadosForemValidos()
    {
        // Arrange
        var clienteRepositoryMock = new Mock<IClienteRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var request = new CadastrarClienteRequest(
            "Bruno Bonetti",
            "12345678900");

        Cliente? clienteAdicionado = null;

        clienteRepositoryMock
            .Setup(repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(false);

        clienteRepositoryMock
            .Setup(repository =>
                repository.AdicionarAsync(
                    It.IsAny<Cliente>(),
                    It.IsAny<CancellationToken>()))
            .Callback<Cliente, CancellationToken>(
                (cliente, _) => clienteAdicionado = cliente)
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(1);

        var handler = new CadastrarClienteHandler(
            clienteRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var response = await handler.HandleAsync(request);

        // Assert
        Assert.NotNull(clienteAdicionado);
        Assert.Equal(request.Nome, clienteAdicionado.Nome);
        Assert.Equal(request.Documento, clienteAdicionado.Documento);

        Assert.Equal(clienteAdicionado.Id, response.Id);
        Assert.Equal(request.Nome, response.Nome);
        Assert.Equal(request.Documento, response.Documento);

        clienteRepositoryMock.Verify(
            repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        clienteRepositoryMock.Verify(
            repository =>
                repository.AdicionarAsync(
                    It.IsAny<Cliente>(),
                    It.IsAny<CancellationToken>()),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork =>
                unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Once);
    }

    [Fact]
    public async Task DeveLancarExcecaoQuandoDocumentoJaEstiverCadastrado()
    {
        // Arrange
        var clienteRepositoryMock = new Mock<IClienteRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var request = new CadastrarClienteRequest(
            "Bruno Bonetti",
            "12345678900");

        clienteRepositoryMock
            .Setup(repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    It.IsAny<CancellationToken>()))
            .ReturnsAsync(true);

        var handler = new CadastrarClienteHandler(
            clienteRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        var excecao = await Assert.ThrowsAsync<InvalidOperationException>(() =>
            handler.HandleAsync(request));

        // Assert
        Assert.Contains(
            "Já existe um cliente cadastrado com este documento.",
            excecao.Message);

        clienteRepositoryMock.Verify(
            repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    It.IsAny<CancellationToken>()),
            Times.Once);

        clienteRepositoryMock.Verify(
            repository =>
                repository.AdicionarAsync(
                    It.IsAny<Cliente>(),
                    It.IsAny<CancellationToken>()),
            Times.Never);

        unitOfWorkMock.Verify(
            unitOfWork =>
                unitOfWork.SaveChangesAsync(
                    It.IsAny<CancellationToken>()),
            Times.Never);
    }

    [Fact]
    public async Task DeveRepassarCancellationTokenParaAsDependencias()
    {
        // Arrange
        var clienteRepositoryMock = new Mock<IClienteRepository>();
        var unitOfWorkMock = new Mock<IUnitOfWork>();

        var request = new CadastrarClienteRequest(
            "Cliente Teste",
            "12345678900");

        using var cancellationTokenSource =
            new CancellationTokenSource();

        var cancellationToken =
            cancellationTokenSource.Token;

        clienteRepositoryMock
            .Setup(repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    cancellationToken))
            .ReturnsAsync(false);

        clienteRepositoryMock
            .Setup(repository =>
                repository.AdicionarAsync(
                    It.IsAny<Cliente>(),
                    cancellationToken))
            .Returns(Task.CompletedTask);

        unitOfWorkMock
            .Setup(unitOfWork =>
                unitOfWork.SaveChangesAsync(
                    cancellationToken))
            .ReturnsAsync(1);

        var handler = new CadastrarClienteHandler(
            clienteRepositoryMock.Object,
            unitOfWorkMock.Object);

        // Act
        await handler.HandleAsync(
            request,
            cancellationToken);

        // Assert
        clienteRepositoryMock.Verify(
            repository =>
                repository.ExistePorDocumentoAsync(
                    request.Documento,
                    cancellationToken),
            Times.Once);

        clienteRepositoryMock.Verify(
            repository =>
                repository.AdicionarAsync(
                    It.IsAny<Cliente>(),
                    cancellationToken),
            Times.Once);

        unitOfWorkMock.Verify(
            unitOfWork =>
                unitOfWork.SaveChangesAsync(
                    cancellationToken),
            Times.Once);
    }
}