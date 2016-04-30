using OrariUnibg.Models;
using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;

namespace OrariUnibg.ViewModels
{
    class OrariGiornoViewModel : NotifyPropertyChangedBase
    {
        #region Private Fields
        private List<CorsoGiornaliero> _listOrari;
		private string _laurea;
        #endregion

        public List<CorsoGiornaliero> ListOrari
        {
            get
            {
                return _listOrari;
            }
            set { SetProperty<List<CorsoGiornaliero>>(ref _listOrari, value, "ListOrari"); }
        }

        public Facolta Facolta { get; set; }
        public String FacoltaString { get { return Facolta.Nome; } }
        public Laurea Laurea { get; set; }
        public String LaureaString 
		{ 
			get 
			{ 
				if (_laurea.ToLower () == "generale")
					return string.Format ("{0} - {1}", FacoltaString, _laurea);
				else
					return _laurea;
			} 
			set { _laurea = value;} 
		}

        public DateTime Data { get; set; }
		public String Day { get { return Data.ToString("dddd", new CultureInfo("it-IT")).ToUpper(); } }
		public String DataString { get { return string.Format("{0}, {1}", Day, Data.ToString("dd'/'MM'/'yyyy")); } }


		#region Public Methods
		public override string ToString ()
		{
			return string.Format("ORARIO GIORNALIERO di {0} \n\n{1}", DataString, string.Join("\n", ListOrari));
		}
		#endregion
    }
}
