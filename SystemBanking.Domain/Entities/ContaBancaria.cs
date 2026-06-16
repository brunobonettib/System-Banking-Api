using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using SystemBanking.Domain.Enums;

namespace SystemBanking.Domain.Entities
{
    public class ContaBancaria
    {
        public Guid Id { get; private set; }

        public string NumeroConta { get; private set; }

        public decimal Saldo { get; private set; }

        public Guid ClienteId { get; private set; }

        public Cliente Cliente { get; private set; }

        private readonly List<Transacao> _transacoes = [];

        public IReadOnlyCollection<Transacao> Transacoes =>
        _transacoes.AsReadOnly();

        public ContaBancaria(string numeroConta, Cliente cliente)
        {
            if (string.IsNullOrWhiteSpace(numeroConta))
                throw new ArgumentException(
                    "Número da conta é obrigatório.",
                    nameof(numeroConta));

            if (cliente is null)
                throw new ArgumentNullException(nameof(cliente));
            Id = Guid.NewGuid();
            NumeroConta = numeroConta;
            Cliente = cliente;
            ClienteId = cliente.Id;
            Saldo = 0;
        }

        public void Depositar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException(
                    "Valor inválido.");

            Saldo += valor;

            _transacoes.Add(
        new Transacao(
            Id,
            valor,
            TipoTransacao.Deposito,
            "Depósito realizado"));
        }

        public void Sacar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException(
                    "Valor inválido.");

            if (valor > Saldo)
                throw new InvalidOperationException(
                    "Saldo insuficiente.");

            Saldo -= valor;

            _transacoes.Add(
        new Transacao(
            Id,
            valor,
            TipoTransacao.Saque,
            "Saque realizado"));
        }

        public void Transferir(
            decimal valor,
            ContaBancaria destino)
        {
            if (destino is null)
                throw new ArgumentNullException(
                    nameof(destino));

            Sacar(valor);

            destino.Depositar(valor);
        }
    }
}