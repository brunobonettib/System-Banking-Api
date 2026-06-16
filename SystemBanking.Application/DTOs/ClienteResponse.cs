using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SystemBanking.Application.DTOs
{
    public record ClienteResponse(Guid Id, string Nome, string Documento)
    {
        
    }
}