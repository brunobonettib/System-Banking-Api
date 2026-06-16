using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemBanking.Domain.Entities;

namespace SystemBanking.Application.Interfaces
{
    public interface IClienteRepository
    {
        void Adicionar(Cliente cliente);

        Cliente? ObterPorId(Guid id);

        IEnumerable<Cliente> Listar();
    }
}