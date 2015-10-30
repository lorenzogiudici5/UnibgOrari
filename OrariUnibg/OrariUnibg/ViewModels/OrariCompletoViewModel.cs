using OrariUnibg.Models;
using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.ViewModels
{
    class OrariCompletoViewModel : NotifyPropertyChangedBase
    {
        #region Private Fields
        private List<CorsoCompleto> _listOrari;
		private string _laurea;
        #endregion

        public List<CorsoCompleto> ListOrari
        {
            get
            {
                return _listOrari;
            }
            set { SetProperty<List<CorsoCompleto>>(ref _listOrari, value, "ListOrari"); }
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

        public String Anno { get; set; }
        public String Semestre { get; set; }
        public bool Group { get; set; }
        public String AnnoSemestre { get { return string.Format("{0}: {1} semestre", Anno, Semestre); } }

		#region Public Methods
		public override string ToString ()
		{
			return string.Format ("ORARIO di {0} - {1} \n\n{2}", LaureaString, AnnoSemestre, string.Join("\n", ListOrari));
		}
		#endregion
    }
}
