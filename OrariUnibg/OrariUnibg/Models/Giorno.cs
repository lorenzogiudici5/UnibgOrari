using OrariUnibg.Services.Database;
using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class Giorno : NotifyPropertyChangedBase
    {
        #region Private Fields
        private IEnumerable<Orari> _listOrari;
        private IEnumerable<Utenza> _listUtenza;
        private DateTime _day;
        private DbSQLite _db;
        #endregion

        public string Day 
        {
            get 
            {
                if (Data == DateTime.Today)
                    return "OGGI";
                else if (Data == DateTime.Today.AddDays(1))
                    return "DOMANI";
                else
                    return Data.ToString("dddd", new CultureInfo("it-IT")).ToUpper();
            }
        }
        public DateTime Data 
        {
            get { return _day; } 
            set
            {
                if (value.ToString("dddd", new CultureInfo("it-IT")).ToLower() == "domenica")
                    _day = value.AddDays(1);
                else
                    _day = value;
            }
        }

        public IEnumerable<Orari> ListaLezioni 
        {
            get
            {
                return _listOrari;
            }
            set { SetProperty<IEnumerable<Orari>>(ref _listOrari, value, "ListaLezioni"); }
        }
        public String DateString { get { return Data.ToString("dd'/'MM'/'yyyy");} }

        public IEnumerable<Utenza> ListUtenza 
        {
            get
            {
                return _listUtenza;
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
