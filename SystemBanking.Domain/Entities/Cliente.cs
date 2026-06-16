using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemBanking.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Documento { get; private set; }

        private readonly List<ContaBancaria> _contas = [];

        public IReadOnlyCollection<ContaBancaria> Contas => _contas.AsReadOnly();

        public Cliente(string nome, string documento)
        {
            Id = Guid.NewGuid();
            Nome = nome;
            Documento = documento;
        }

        public void AdicionarConta(ContaBancaria conta)
        {
            _contas.Add(conta);
        }
    }
}