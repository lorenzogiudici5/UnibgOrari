using OrariUnibg.Services.Database;
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
        private string _note;
		private DateTime _date;
        public CorsoGiornaliero(){ }
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
		public DateTime Date 
		{ 
			get { return _date;}
			set { _date = value.Date;}
//			set { _date = value.Date.AddDays(1);} //perchè quando inserisco nel database, mi togli un giorno!!!****
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

		#region PublicMethods
		public override string ToString ()
		{
			return string.Format ("{0}: {1} ({2}), ", Ora, InsegnamentoToString(), Aula);
		}
		#endregion

    }
		
}
