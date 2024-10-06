using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class Publicacao
    {
        public int IdPublicacao { get; set; }
        public DateTime DataPublicacao { get; set; }
        public string TituloPublicacao { get; set; } = string.Empty;
        public string TextoPublicacao { get; set; } = string.Empty;
    }
}