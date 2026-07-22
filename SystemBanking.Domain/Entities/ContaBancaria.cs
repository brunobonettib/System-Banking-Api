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
                    "O valor do depósito deve ser maior que zero.",
                    nameof(valor));

            Saldo += valor;

            RegistrarTransacao(
                valor,
                TipoTransacao.Deposito,
                "Depósito realizado");
        }

        public void Sacar(decimal valor)
        {
            if (valor <= 0)
                throw new ArgumentException(
                    "O valor do saque deve ser maior que zero.",
                    nameof(valor));

            if (valor > Saldo)
                throw new InvalidOperationException(
                    "Saldo insuficiente para realizar o saque.");

            Saldo -= valor;

            RegistrarTransacao(
                valor,
                TipoTransacao.Saque,
                "Saque realizado");
        }

        public void Transferir(decimal valor, ContaBancaria destino)
        {
            if (destino is null)
                throw new ArgumentNullException(nameof(destino));

            if (valor <= 0)
                throw new ArgumentException(
                    "O valor da transferência deve ser maior que zero.",
                    nameof(valor));

            if (valor > Saldo)
                throw new InvalidOperationException(
                    "Saldo insuficiente para realizar a transferência.");

            if (destino.Id == Id)
                throw new InvalidOperationException(
                    "A conta de destino deve ser diferente da conta de origem.");

            Saldo -= valor;

            RegistrarTransacao(
                valor,
                TipoTransacao.Transferencia,
                $"Transferência enviada para a conta {destino.NumeroConta}");

            destino.ReceberTransferencia(valor, NumeroConta);
        }

        private void ReceberTransferencia(decimal valor, string numeroContaOrigem)
        {
            Saldo += valor;

            RegistrarTransacao(
                valor,
                TipoTransacao.Transferencia,
                $"Transferência recebida da conta {numeroContaOrigem}");
        }

        private void RegistrarTransacao(
            decimal valor,
            TipoTransacao tipo,
            string descricao)
        {
            _transacoes.Add(
                new Transacao(
                    Id,
                    valor,
                    tipo,
                    descricao));
        }
    }
}