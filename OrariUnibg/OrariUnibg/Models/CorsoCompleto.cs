using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class CorsoCompleto : Corso
    {
        private string _inizio;
        private string _fine;
        private string _inizioFine;
        public CorsoCompleto()
        {

        }
       
        public List<Lezione> Lezioni { get { return _lezioni; } }
        public List<Lezione> _lezioni = new List<Lezione>(); 
        
        public string InizioFine 
        {
            get { return _inizioFine; }
            set 
            { 
                _inizio = value.Substring(0, 10);
                _fine = value.Substring(10, 10);
                _inizioFine = _inizio + " - " + _fine;
            }
        }
    }
}
