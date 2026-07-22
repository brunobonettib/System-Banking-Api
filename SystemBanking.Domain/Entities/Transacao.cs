using SystemBanking.Domain.Enums;

namespace SystemBanking.Domain.Entities
{
    public class Transacao
    {
        public Guid Id { get; private set; }

        public Guid ContaBancariaId { get; private set; }

        public decimal Valor { get; private set; }

        public TipoTransacao Tipo { get; private set; }

        public DateTime Data { get; private set; }

        public string Descricao { get; private set; }

        public Transacao(Guid contaBancariaId, decimal valor, TipoTransacao tipo, string descricao)
        {
            if (contaBancariaId == Guid.Empty)
                throw new ArgumentException(
                    "A conta bancária é obrigatória.",
                    nameof(contaBancariaId));

            if (valor <= 0)
                throw new ArgumentException(
                    "O valor da transação deve ser maior que zero.",
                    nameof(valor));

            if (!Enum.IsDefined(tipo))
                throw new ArgumentOutOfRangeException(
                    nameof(tipo),
                    "Tipo de transação inválido.");

            if (string.IsNullOrWhiteSpace(descricao))
                throw new ArgumentException(
                    "A descrição da transação é obrigatória.",
                    nameof(descricao));

            Id = Guid.NewGuid();
            ContaBancariaId = contaBancariaId;
            Valor = valor;
            Tipo = tipo;
            Descricao = descricao.Trim();
            Data = DateTime.UtcNow;
        }
    }
}