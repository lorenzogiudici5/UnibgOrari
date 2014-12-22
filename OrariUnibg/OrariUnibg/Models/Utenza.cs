using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Utenza
    {
        private string _aula;
        private string _ora;
        private string _aulaOra;

        public String AulaOra 
        {
            get { return _aulaOra; }
            set
            {
                if (value != null)
                {
                    _aula = value.Substring(0, value.Length - 11);
                    _ora = value.Substring(_aula.Length);
                    if (_aula.Substring(_aula.Length - 1, 1) == " ")
                        _aulaOra = _aula + _ora;
                    else
                        _aulaOra = _aula + " " + _ora;
                }

            }  
        }

        public string Aula
        {
            get { return _aula; }
        }
        public string Ora
        {
            get { return _ora; }
        }
        public DateTime Data { get; set; }
        public String DateString { get { return Data.ToString("dd'/'MM'/'yyyy"); } }
    }
}
