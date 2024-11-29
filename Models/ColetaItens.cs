using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EcocriaMVC.Models.Enum;

namespace EcocriaMVC.Models
{
    public class ColetaItens
    {
        public int IdItemColeta { get; set; }
        public int QuantidadeColeta { get; set; }
        public int? IdColeta { get; set; }
        public int? IdMaterial { get; set; }
        public int? IdOrdemGrandeza { get; set; }
        public Coletas? Coletas { get; set; }
        public MateriaisEnum? Materiais { get; set; }
        public OrdemDeGrandezaEnum? OrdemDeGrandeza { get; set; }
    }
}