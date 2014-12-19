using OrariUnibg.Services.Database;
using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    class Giorno : NotifyPropertyChangedBase
    {
        #region Private Fields
        private IEnumerable<Orari> _listOrari;
        private IEnumerable<Utenza> _listUtenza;
        private string _aula;
        private string _ora;
        private string _aulaOra;
        #endregion

        public string Day
        {
            get;
            set;
        }
        public DateTime Data { get; set; }

        public IEnumerable<Orari> ListaLezioni 
        {
            get
            {
                return _listOrari;
                //db = new DbSQLite();
                //var l =  db.GetItems().OrderBy(x => x.Ora).Where(dateX => DateTime.Compare(Data, dateX.Date) == 0);
                //return l;
            }
            set { SetProperty<IEnumerable<Orari>>(ref _listOrari, value, "ListaLezioni"); }
        }
        public String DateString { get { return Data.ToString("dd'/'MM'/'yyyy");} }

        public IEnumerable<Utenza> ListUtenza 
        {
            get
            {
                return _listUtenza;
                //db = new DbSQLite();
                //var l =  db.GetItems().OrderBy(x => x.Ora).Where(dateX => DateTime.Compare(Data, dateX.Date) == 0);
                //return l;
            }
            set { SetProperty<IEnumerable<Utenza>>(ref _listUtenza, value, "ListUtenza"); }
        }
        public String UsoUtenza 
        {
            get 
            {
                if (ListUtenza.Count() > 0)
                    return ListUtenza.FirstOrDefault().Aulaora;
                else
                    return string.Empty;
            }
        }
    }
}
