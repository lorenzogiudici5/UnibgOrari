using OrariUnibg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.ViewModels
{
    class CorsoCompletoGroupViewModel : Corso
    {
        #region Private Fields
        private string _inizio;
        private string _fine;
        private string _inizioFine;
        private string _aula;
        private string _ora;
        private string _aulaOra;
        private string _note;
        #endregion

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

        public DateTime Inizio
        {
            get { return new DateTime(int.Parse(_inizio.Split('/')[2]), int.Parse(_inizio.Split('/')[1]), int.Parse(_inizio.Split('/')[0])); }
        }

        public DateTime Fine
        {
            get { return new DateTime(int.Parse(_fine.Split('/')[2]), int.Parse(_fine.Split('/')[1]), int.Parse(_fine.Split('/')[0])); }
        }

        public int Semestre
        {
            get
            {
                if (Inizio.Month > 8)
                    return 1;
                else return 2;
            }
        }

        public string AulaOra
        {
            get { return _aulaOra; }
            set
            {
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

        public bool IsVisible { get; set; }
        public OrariUnibg.Models.Lezione.Day Giorno { get; set; }
        public string Note
        {
            get { return _note; }
            set
            {
                if (value == null)
                    _note = string.Empty;
                else
                    _note = value;
            }
        }
        public int Day { get; set; }

        //lez.isVisible, lez.Giorno, lez.Note, lez.day
    }
}
