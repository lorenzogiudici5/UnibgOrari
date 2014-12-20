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
            System.Diagnostics.Debug.WriteLine("COUNT: " + _db.GetAllMieiCorsi().Count());
            this.Title = "Home";
            BackgroundColor = ColorHelper.White;

            if(DateTime.Now.Hour < 18)
            {
                _dateOggi = DateTime.Today;
                _dateDomani = _dateOggi.AddDays(1);
                _dateDopodomani = _dateOggi.AddDays(2);
            }
            else
            {
                _dateOggi = DateTime.Today.AddDays(1);
                _dateDomani = _dateOggi.AddDays(1);
                _dateDopodomani = _dateDomani.AddDays(1);
            }

            _oggi = new Giorno() { Data = DateTime.Today };
            _oggi.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data, dateX.Date) == 0);
            _oggi.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _oggi.Data);
            
            _domani = new Giorno() { Data = _oggi.Data.AddDays(1)};
            _domani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data, dateX.Date) == 0);
            _domani.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _domani.Data);
            
            _dopodomani = new Giorno() { Data = _domani.Data.AddDays(1)};
            _dopodomani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dopodomani.Data, dateX.Date) == 0);
            _dopodomani.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _dopodomani.Data);
            //var oggi = new Giorno() { Data = DateTime.Today };
            //var domani = new Giorno() { Data = oggi.Data.AddDays(1) };
            //var dopodomani = new Giorno() { Data = domani.Data.AddDays(1)};

            list = new List<Giorno>()
            {
                _oggi, _domani, _dopodomani
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

                LoadListGiorno();
                this.ItemsSource = list;
                MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", false);

                //DependencyService.Get<INotification>().BackgroundSync();
            }, 0, 0); 

            ToolbarItems.Add(tbiSync);

            MessagingCenter.Subscribe<TabbedDayView>(this, "delete_corso", deleteMioCorso);
        }
        #endregion

        #region Private Fields
        private DateTime _dateOggi;
        private DateTime _dateDomani;
        private DateTime _dateDopodomani;
        private DbSQLite _db;
        private List<Giorno> list;
        private Giorno _oggi;
        private Giorno _domani;
        private Giorno _dopodomani;
        #endregion

        #region Private Methods

        private void LoadListGiorno()
        {
            _oggi.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data, dateX.Date) == 0);
            _oggi.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _oggi.Data);

            _domani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data, dateX.Date) == 0);
            _domani.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _domani.Data);

            _dopodomani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dopodomani.Data, dateX.Date) == 0);
            _dopodomani.ListUtenza = _db.GetAllUtenze().Where(x => x.Data == _dopodomani.Data);
        }
        protected override void OnAppearing()
        {
            //dovrebbe scaricare i nuovi orari
            LoadListGiorno();
            base.OnAppearing();
        }
        #endregion

        #region Event Handlers
        private void deleteMioCorso(TabbedDayView obj)
        {
            LoadListGiorno();
        }
        #endregion
    }
}
