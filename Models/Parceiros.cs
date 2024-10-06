using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class Parceiros
    {
        public int IdParceiro { get; set; }
        public string NomeParceiro { get; set; } = string.Empty;
        public Boolean StatusParceiro { get; set; }
        public double DoacaoParceiro { get; set; }
        public DateTime DataDoacao { get; set; }



        public int? IdUsuario { get; set; }  

        [JsonIgnore]
        public Usuario? Usuario { get; set; }
    }
}