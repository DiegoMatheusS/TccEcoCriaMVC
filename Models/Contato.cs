using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcocriaMVC.Models
{
    public class Contato
    {
        public int IdContato { get; set; }
        public string Nome { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Telefone { get; set; } = string.Empty;
        public string Cidade { get; set; } = string.Empty;
        public string UF { get; set; } = string.Empty;
        public string Assunto { get; set; } = string.Empty;
        public string Mensagem { get; set; } = string.Empty;
    }
}