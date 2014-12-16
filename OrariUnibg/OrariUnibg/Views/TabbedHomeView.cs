using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Services.Database;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views
{
    class TabbedHomeView : TabbedPage
    {
        #region Constructor
        public TabbedHomeView()
        {
            _db = new DbSQLite();
            System.Diagnostics.Debug.WriteLine("COUNT: " + _db.GetItemsMieiCorsi().Count());
            this.Title = "Home";
            BackgroundColor = ColorHelper.White;

            if(DateTime.Now.Hour < 18)
            {
                _oggi = "OGGI";
                _dateOggi = DateTime.Today;
                _domani = "DOMANI";
                _dateDomani = DateTime.Today.AddDays(1);
                _dateDopodomani = DateTime.Today.AddDays(2);
                _dopodomani = _dateDopodomani.ToString("dddd", new CultureInfo("it-IT")).ToUpper();
            }
            else
            {
                _oggi = "DOMANI";
                _dateOggi = DateTime.Today.AddDays(1);
                _dateDomani = DateTime.Today.AddDays(2);
                _domani = _dateDomani.ToString("dddd", new CultureInfo("it-IT")).ToUpper();
                _dateDopodomani = DateTime.Today.AddDays(3);
                _dopodomani = _dateDopodomani.ToString("dddd", new CultureInfo("it-IT")).ToUpper();
            }

            List<Giorno> list = new List<Giorno>()
            {
                new Giorno() {Day = _oggi, Data = _dateOggi, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateOggi, dateX.Date) == 0)},
                new Giorno() {Day = _domani, Data = _dateDomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDomani, dateX.Date) == 0)},
                new Giorno() {Day = _dopodomani, Data =_dateDopodomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDopodomani, dateX.Date) == 0) },

                //new Giorno() {Day = _oggi, Data = _dateOggi},
                //new Giorno() {Day = _domani, Data = _dateDomani},
                //new Giorno() {Day = _dopodomani, Data =_dateDpodomani },
            };
            this.ItemsSource = list;
            

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new TabbedDayView();
            });

            ToolbarItem tbiSync = new ToolbarItem("Sync", "ic_sync.png", async () =>
            {
                var _listOrariGiorno = _db.GetAllOrari();
                foreach (var l in _listOrariGiorno)
                {
                    if (l.Date < _dateOggi)
                        _db.DeleteSingleOrari(l.Id);
                };

                foreach (var x in list)
                {
                    string s = await Web.GetOrarioGiornaliero("IN", 1, 0, x.DateString);
                    List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, x.Data);

                    foreach (var l in listaCorsi)
                    {
                        if (_db.CheckAppartieneMieiCorsi(l))
                        {
                            _db.InsertUpdate(l);
                        }
                    }

                    list = new List<Giorno>()
                    {
                        new Giorno() {Day = _oggi, Data = _dateOggi, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateOggi, dateX.Date) == 0)},
                        new Giorno() {Day = _domani, Data = _dateDomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDomani, dateX.Date) == 0)},
                        new Giorno() {Day = _dopodomani, Data =_dateDopodomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDopodomani, dateX.Date) == 0) },
                    };
                    this.ItemsSource = list;
                    // MessagingCenter.Send<TabbedHomeView>(this, "sync");

                }
                    //DependencyService.Get<INotification>().Notify();
            }, 0, 0); 
            ToolbarItems.Add(tbiSync);
        }
        #endregion

        #region Private Fields
        private String _oggi;
        private String _domani;
        private String _dopodomani;
        private DateTime _dateOggi;
        private DateTime _dateDomani;
        private DateTime _dateDopodomani;
        private DbSQLite _db;
        #endregion

        #region Private Methods

        #endregion
    }
}
