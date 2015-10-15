using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Corso
    {
        public string Insegnamento { get; set; }
        public string Codice { get; set; }
        public string Docente { get; set; }

		public String InsegnamentoToString()
		{
			var stringa = Insegnamento.Split ('(');
			return stringa [0];
		}
    }
}
