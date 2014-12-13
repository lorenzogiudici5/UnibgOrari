using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Models
{
    public class CorsoGiornaliero : Corso
    {
        private string _aula;
        private string _ora;
        private string _aulaOra;
        public CorsoGiornaliero()
        {

        }
        public string AulaOra 
        {
            get { return _aulaOra; }
            set {
                if (value != null)
                {
                    _aula = value.Substring(0, value.Length - 11);
                    _ora = value.Substring(Aula.Length);
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
        public string Note { get; set; }
        public DateTime Date { get; set; }

    }
}
