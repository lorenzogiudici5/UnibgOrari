﻿using OrariUnibg.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class CorsoCompleto : Corso
    {
        #region Private Fields
        private string _inizio;
        private string _fine;
        private string _inizioFine;
        #endregion

        public List<Lezione> Lezioni
        {
            get { return _lezioni; }
            set { if (value != _lezioni) _lezioni = value; }
        }
        public List<Lezione> _lezioni = new List<Lezione>(); 
        
        public string InizioFine 
        {
            get { return _inizioFine; }
            set 
            { 
                if(_inizioFine == null)
                {
                    var lenght = value.Length;
                    _inizio = value.Substring(0, 10);
                    _fine = value.Substring(lenght - 10, 10);
                    _inizioFine = _inizio + " - " + _fine;
                }

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

        public bool MioCorso
        {
            get
            {
                var _db = new DbSQLite();
                if (_db.CheckAppartieneMieiCorsi(this))
                    return true;
                else return false;
            }
        }

		public override string ToString ()
		{
			return string.Format ("{0} ({1}) \n{2}\n", InsegnamentoToString(), Docente, string.Join("\n", Lezioni.Where(x => x.AulaOra != string.Empty)));
		}
    }

    public class CorsoCompletoAlgoritmo : CorsoCompleto
    {
        public int Punteggio { get; set; }
    }
}
