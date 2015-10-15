using OrariUnibg.Models;
using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.ViewModels
{
    class OrariGiornoViewModel : NotifyPropertyChangedBase
    {
        #region Private Fields
        private List<CorsoGiornaliero> _listOrari;
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
        public String LaureaString { get; set; }

        public DateTime Data { get; set; }
        public String DataString { get { return string.Format("ORARIO DEL GIORNO: {0}", Data.ToString("dd'/'MM'/'yyyy")); } }


		#region Public Methods
		public override string ToString ()
		{
			return string.Format("{0} \n\n{1}", DataString, string.Join("\n", ListOrari));
		}
		#endregion
    }
}
