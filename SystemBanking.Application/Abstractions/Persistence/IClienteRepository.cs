using SystemBanking.Domain.Entities;

namespace SystemBanking.Application.Abstractions.Persistence;

public interface IClienteRepository
{
    Task<bool> ExistePorDocumentoAsync(string documento, CancellationToken cancellationToken = default);

    Task AdicionarAsync(Cliente cliente, CancellationToken cancellationToken = default);
}