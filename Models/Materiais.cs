using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcoCriaMVC.Models.Enuns;

namespace EcoCriaMVC.Models
{
    public class Materiais
    {
        public int IdMaterial { get; set; }
        public string NomeMaterial { get; set; } = string.Empty;
        public MateriaisEnun Material { get; set; }
    }
}