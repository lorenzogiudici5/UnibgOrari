using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Lezione
    {
        private string _aula = string.Empty;
        private string _ora = string.Empty;
        private string _aulaOra = string.Empty;
        private string _note = string.Empty;
        public enum Day
        {
            Lunedi = 0, Martedi = 1, Mercoledi = 2, Giovedi = 3, Venerdi = 4, Sabato = 5
        }
        public int _day;
        public Lezione()
        {

        }
        public int day { get { return _day; } }
        public Day Giorno { get; set; }
        public string AulaOra
        {
            get { return _aulaOra; }
            set
            {
                if (value != string.Empty)
                {
                    _aula = value.Substring(0, value.Length - 11);
                    _ora = value.Substring(Aula.Length);
                    _aulaOra = _aula + " " + _ora;
                }
                else
                    _aulaOra = string.Empty;

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

        public string Note { get { return _note; } set { } }

        public bool isVisible 
        {
            get
            {
                if (_aulaOra == string.Empty) return false;
                else return true;
            }
        }
        //public string Aula
        //{
        //    get 
        //    {
        //        if (AulaOra != string.Empty)
        //            return AulaOra.Substring(0, AulaOra.Length - 11);
        //        else 
        //            return string.Empty;
        //    }
        //    set { }
        //}
        //public string Ora
        //{
        //    get 
        //    {
        //        if (AulaOra != string.Empty)
        //            return AulaOra.Substring(Aula.Length);
        //        else
        //            return string.Empty;
        //    }
        //    set { }
        //}
    }
}
