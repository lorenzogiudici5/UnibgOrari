using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Corso
    {
        [Newtonsoft.Json.JsonProperty("Id")]
        public string Id { get; set; }

        public string Insegnamento { get; set; }
        public string Codice { get; set; }
        public string Docente { get; set; }

        #region Serializer Json
        public virtual bool ShouldSerializeId()
        {
            return true;
        }
        public virtual bool ShouldSerializeInsegnamento()
        {
            return true;
        }
        public virtual bool ShouldSerializeCodice()
        {
            return true;
        }
        public virtual bool ShouldSerializeDocente()
        {
            return true;
        }
        #endregion

        #region Methods
        public string InsegnamentoToString()
		{
			var stringa = Insegnamento.Split ('(');
			return stringa [0];
		}
        #endregion

    }
}
