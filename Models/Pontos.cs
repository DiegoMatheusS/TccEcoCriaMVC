using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class Pontos
    {
        public int IdPonto { get; set; }
        public string NomePonto { get; set; } = string.Empty;
        public string EnderecoPonto { get; set; } = string.Empty;
        public int CepEndereco { get; set; }
        public string UfEndereco { get; set; } = string.Empty;
        public string CidadeEndereco { get; set; } = string.Empty;

        public TipoDePonto TipoDePonto {get; set; } = null!;
        public int IdTipoPonto {get; set; }
    }
}