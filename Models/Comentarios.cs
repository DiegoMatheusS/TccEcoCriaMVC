using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EcoCriaMVC.Models
{
    public class Comentarios
    {
        public int IdComentario { get; set; }
        public DateTime MomentoComentario { get; set; }
        public string TextoComentario { get; set; } = string.Empty;
    }
}