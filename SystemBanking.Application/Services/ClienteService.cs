using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemBanking.Application.DTOs;
using SystemBanking.Application.Interfaces;
using SystemBanking.Domain.Entities;

namespace SystemBanking.Application.Services
{
    public class ClienteService
    {
        private readonly IClienteRepository _clienteRepository;

        public ClienteService(IClienteRepository clienteRepository)
        {
            _clienteRepository = clienteRepository;
        }

        public ClienteResponse Criar(CriarClienteRequest request)
        {
            var cliente = new Cliente(
                request.Nome,
                request.Documento);

            _clienteRepository.Adicionar(cliente);

            return new ClienteResponse(
                cliente.Id,
                cliente.Nome,
                cliente.Documento);
        }

        public IEnumerable<ClienteResponse> Listar()
        {
            return _clienteRepository
                .Listar()
                .Select(cliente => new ClienteResponse(
                    cliente.Id,
                    cliente.Nome,
                    cliente.Documento));
        }
    }
}