using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemBanking.Application.DTOs
{
    public record CriarClienteRequest(string Nome, string Documento)
    {

    }
}