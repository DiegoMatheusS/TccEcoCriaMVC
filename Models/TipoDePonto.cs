using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class TipoDePonto
    {
        public int IdTipoPonto { get; set; }
        public string DescricaoTipoPonto { get; set; } = string.Empty;
        public Boolean StatusTipoPonto { get; set; }

        [JsonIgnore]
        public Pontos? Pontos {get; set;}
    }
}