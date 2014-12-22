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
        public String LaureaString { get; set; }

        public String Anno { get; set; }
        public String Semestre { get; set; }
        public bool Group { get; set; }
        public String AnnoSemestre { get { return string.Format("{0}: {1} semestre", Anno, Semestre); } }
    }
}
