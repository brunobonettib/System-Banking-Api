using SystemBanking.Application.Abstractions.Persistence;
using SystemBanking.Domain.Entities;

namespace SystemBanking.Application.Clientes.CadastrarCliente;

public sealed class CadastrarClienteHandler
{
    private readonly IClienteRepository _clienteRepository;
    private readonly IUnitOfWork _unitOfWork;

    public CadastrarClienteHandler(IClienteRepository clienteRepository, IUnitOfWork unitOfWork)
    {
        _clienteRepository = clienteRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<CadastrarClienteResponse> HandleAsync(CadastrarClienteRequest request, CancellationToken cancellationToken = default)
    {
        var documentoJaCadastrado =
            await _clienteRepository.ExistePorDocumentoAsync(
                request.Documento,
                cancellationToken);

        if (documentoJaCadastrado)
        {
            throw new InvalidOperationException(
                "Já existe um cliente cadastrado com este documento.");
        }

        var cliente = new Cliente(
            request.Nome,
            request.Documento);

        await _clienteRepository.AdicionarAsync(
            cliente,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new CadastrarClienteResponse(
            cliente.Id,
            cliente.Nome,
            cliente.Documento);
    }
}