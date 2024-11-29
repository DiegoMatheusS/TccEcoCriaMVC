using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcocriaMVC.Models
{
    public class Pontos
    {
        public int IdPonto { get; set; }
        public string NomePonto { get; set; } = string.Empty;
        public string EnderecoPonto { get; set; } = string.Empty;
        public string CepEndereco { get; set; } = string.Empty;
        public string UfEndereco { get; set; } = string.Empty;
        public string CidadeEndereco { get; set; } = string.Empty;
        public double Latitude { get; set; } 
        public double Longitude { get; set; } 
        
    }
}