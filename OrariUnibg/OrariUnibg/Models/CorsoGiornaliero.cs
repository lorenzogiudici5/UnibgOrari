using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Models
{
    public class CorsoGiornaliero
    {
        private string _aula;
        private string _ora;
        private string _aulaOra;
        public CorsoGiornaliero()
        {

        }
        public string Insegnamento { get; set; }
        public string Cod {get; set;}
        public string Docente { get; set; }
        public string AulaOra 
        {
            get { return _aulaOra; }
            set {
                _aula = value.Substring(0, value.Length - 11);
                _ora = value.Substring(Aula.Length);
                _aulaOra = _aula + " " + _ora;
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

        //public NoteLezioni NoteBackground 
        //{ 
        //    get 
        //    {
        //        switch (Note)
        //        {
        //            case
        //        }
        //    } 
        //}
        //public Color Background 
        //{ 
        //    get 
        //    {
        //        switch (Note)
        //        {
        //            case "Sospensione lezione":
        //                return Color.FromHex("FF6666");
        //            case "Cambio aula":
        //                return Color.FromHex("FFFF66");
        //            case "Attività accademica":
        //                return Color.FromHex("B0B0FF");
        //            case "Attività integrativa":
        //                return Color.Pink;
        //            case "Esame":
        //                return Color.FromHex("A0FFA0");
        //            case "Alta formazione":
        //                return Color.FromHex("A0FFFF");
        //            case "Recupero lezione":
        //                return Color.FromHex("00DD00");
        //            default: 
        //                return Color.Transparent;
        //        }
        //    }
        //    set { }
        //}
    }
}
