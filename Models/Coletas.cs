using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using EcoCriaMVC.Models;

namespace EcocriaMVC.Models
{
    public class Coletas
    {
        public int IdColeta { get; set; }
        public DateTime MomentoColeta { get; set; }
        public int? IdPonto {get; set;}
        public int? IdUsuario { get; set; }
        public Pontos? Pontos { get; set; }
        public Usuario? Usuario { get; set; }

        [JsonIgnore]
        public List<ColetaItens> ColetaItens {get; set;} = new List<ColetaItens>();


        [JsonIgnore]  // Ignora o campo original para não ser serializado
        public string FormattedMomentoColeta => MomentoColeta.ToString("dd/MM/yyyy HH:mm:ss");
    }
}