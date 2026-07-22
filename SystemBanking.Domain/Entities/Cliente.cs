namespace SystemBanking.Domain.Entities
{
    public class Cliente
    {
        public Guid Id { get; private set; }

        public string Nome { get; private set; }

        public string Documento { get; private set; }

        private readonly List<ContaBancaria> _contas = [];

        public IReadOnlyCollection<ContaBancaria> Contas =>
            _contas.AsReadOnly();

        public Cliente(string nome, string documento)
        {
            if (string.IsNullOrWhiteSpace(nome))
                throw new ArgumentException(
                    "O nome do cliente é obrigatório.",
                    nameof(nome));

            if (string.IsNullOrWhiteSpace(documento))
                throw new ArgumentException(
                    "O documento do cliente é obrigatório.",
                    nameof(documento));

            Id = Guid.NewGuid();
            Nome = nome.Trim();
            Documento = documento.Trim();
        }

        public void AdicionarConta(ContaBancaria conta)
        {
            if (conta is null)
                throw new ArgumentNullException(nameof(conta));

            if (conta.ClienteId != Id)
                throw new InvalidOperationException(
                    "A conta deve pertencer a este cliente.");

            if (_contas.Any(
                    x => x.NumeroConta == conta.NumeroConta))
                throw new InvalidOperationException(
                    "Já existe uma conta com esse número para o cliente.");

            _contas.Add(conta);
        }
    }
}