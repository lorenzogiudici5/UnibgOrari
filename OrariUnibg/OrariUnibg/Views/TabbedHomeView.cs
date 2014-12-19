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
                new Giorno() {Day = _oggi, Data = _dateOggi, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateOggi, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _dateOggi)},
                new Giorno() {Day = _domani, Data = _dateDomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDomani, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _dateDomani)},
                new Giorno() {Day = _dopodomani, Data =_dateDopodomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDopodomani, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _dateDopodomani) },

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
                MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", true);
                var _listOrariGiorno = _db.GetAllOrari();
                foreach (var l in _listOrariGiorno)
                {
                    if (l.Date < _dateOggi)
                        _db.DeleteSingleOrari(l.Id);
                };

                foreach (var d in list)
                {
                    //CHECK CORSI
                    string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, Settings.Laurea, d.DateString);
                    List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d.Data);

                    foreach (var l in listaCorsi)
                    {
                        var corso = l;

                        if (_db.CheckAppartieneMieiCorsi(l))
                        {
                            //_db.InsertUpdate(l);
                            var orario = new Orari()
                            {
                                Insegnamento = corso.Insegnamento,
                                Codice = corso.Codice,
                                AulaOra = corso.AulaOra,
                                Note = corso.Note,
                                Date = corso.Date,
                                Docente = corso.Docente,
                            };

                            if (_db.AppartieneOrari(orario)) //l'orario è già presente
                            {
                                var o = _db.GetAllOrari().FirstOrDefault(y => y.Insegnamento == orario.Insegnamento && y.Date == orario.Date);
                                if ((string.Compare(o.Note, corso.Note) != 0) || !o.Notify)
                                {
                                    o.Note = corso.Note;
                                    o.AulaOra = corso.AulaOra;
                                    if (o.Note != null && o.Note != "" && !o.Notify)
                                    {
                                        DependencyService.Get<INotification>().SendNotification(corso);
                                        //SendNotification(corso);
                                        o.Notify = true;
                                    }
                                    _db.Update(o);
                                }
                            }
                            else // l'orario non è presente nel mio db
                            {
                                orario.Notify = false;

                                if (orario.Note != null && orario.Note != "" && !orario.Notify)
                                {
                                    DependencyService.Get<INotification>().SendNotification(corso);
                                    //SendNotification(corso);
                                    orario.Notify = true;
                                }

                                _db.Insert(orario);
                            }
                        }
                    }

                    //CHECK USO UTENZA
                    string s_ut = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, 0, d.DateString);
                    List<CorsoGiornaliero> listaUtenze = Web.GetSingleOrarioGiornaliero(s, 0, d.Data);

                    foreach (var u in listaUtenze)
                    {
                        var utenza = u;
                        if (utenza.Insegnamento.Contains("Utenza"))
                            _db.Insert(new Utenza() { Data = utenza.Date, Aulaora = utenza.AulaOra });
                    }
                }

                list = new List<Giorno>()
                {
                    new Giorno() {Day = _oggi, Data = _dateOggi, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateOggi, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(z => z.Data == _dateOggi)},
                    new Giorno() {Day = _domani, Data = _dateDomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDomani, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(z => z.Data == _dateDomani)},
                    new Giorno() {Day = _dopodomani, Data =_dateDopodomani, ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dateDopodomani, dateX.Date) == 0), ListUtenza = _db.GetAllUtenze().Where(z => z.Data == _dateDopodomani) },

                };
                this.ItemsSource = list;
                MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", false);

                DependencyService.Get<INotification>().BackgroundSync();
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
