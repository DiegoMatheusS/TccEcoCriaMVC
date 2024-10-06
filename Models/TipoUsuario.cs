using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class TipoUsuario
    {
        public int IdTipoUsuario { get; set; }
        public string DescricaoTipoUsuario { get; set; } = string.Empty;
    }
}