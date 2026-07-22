using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
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
            Id = Guid.NewGuid();
            ContaBancariaId = contaBancariaId;
            Valor = valor;
            Tipo = tipo;
            Descricao = descricao;
            Data = DateTime.UtcNow;
        }
    }
}