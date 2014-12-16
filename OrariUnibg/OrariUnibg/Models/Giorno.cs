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
        private DbSQLite db;
        private IEnumerable<Orari> _list;
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
                return _list;
                //db = new DbSQLite();
                //var l =  db.GetItems().OrderBy(x => x.Ora).Where(dateX => DateTime.Compare(Data, dateX.Date) == 0);
                //return l;
            }
            set { SetProperty<IEnumerable<Orari>>(ref _list, value, "ListaLezioni"); }
        }
        public String DateString { get { return Data.ToString("dd'/'MM'/'yyyy");} }
    }
}
