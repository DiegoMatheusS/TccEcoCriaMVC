using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class Premios
    {
        public int IdPremio { get; set; }
        public string DescricaoPremio { get; set; } = string.Empty;
        public int QuantidadePremio { get; set; }
        public int PontosPremio { get; set; }
    }
}