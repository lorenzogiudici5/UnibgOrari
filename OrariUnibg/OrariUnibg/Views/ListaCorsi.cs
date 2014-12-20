using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Services.Database;
using OrariUnibg.Views.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class ListaCorsi : ContentPage
    {
        #region Constructor
        public ListaCorsi(List<CorsoCompleto> lista_completo)
        {
            Title = "Corsi";
            BackgroundColor = ColorHelper.White;
            _listSource = lista_completo;
            Content = getView();
        }
        #endregion

        #region Private Fields
        private List<CorsoCompleto> _listSource;
        private List<MieiCorsi> _preferiti;
        private ListView _list;
        private DbSQLite _db;
        //private DateTime _dateOggi;
        //private DateTime _dateDomani;
        //private DateTime _dateDopodomani;
        private ActivityIndicator _activityIndicator;
        private Giorno _oggi;
        private Giorno _domani;
        private Giorno _dopodomani;
        #endregion

        #region Private Methods
        private View getView()
        {
            _db = new DbSQLite();
            var _listaGroup = _listSource.GroupBy(x => x.Semestre);
            _list = new ListView()
            {
                ItemsSource = _listaGroup,
                ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup)),
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
            };
            if (Device.OS != TargetPlatform.WinPhone)
                _list.GroupHeaderTemplate = new DataTemplate(typeof(HeaderSemestreCell));
            _preferiti = new List<MieiCorsi>();

            _list.ItemSelected += _list_ItemSelected;

            _activityIndicator = new ActivityIndicator()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                IsRunning = false,
                IsVisible = false
            };

            ToolbarItem tbiNext = new ToolbarItem("Avanti", "ic_next.png", toolbarItem_next, 0, 0);

            ToolbarItems.Add(tbiNext);

            var layout = new StackLayout()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 5,
                Children = { _list, _activityIndicator}
            };
            return layout;
        }

        void _list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var selected = (CorsoCompleto)_list.SelectedItem;

            MieiCorsi newFavourite = new MieiCorsi() { Codice = selected.Codice, Insegnamento = selected.Insegnamento, Docente = selected.Docente };

            int index = _preferiti.FindIndex(f => f.Insegnamento == newFavourite.Insegnamento);
            if (index >= 0)
            {
                MessagingCenter.Send<ListaCorsi, MieiCorsi>(this, "deselect_fav", newFavourite);
                _preferiti.RemoveAt(index);
            }
            else
            {
                MessagingCenter.Send<ListaCorsi, MieiCorsi>(this, "select_fav", newFavourite);
                _preferiti.Add(newFavourite);
            }

            ((ListView)sender).SelectedItem = null;
        }
        #endregion

        #region Event Handlers
        private async void toolbarItem_next()
        {
            _activityIndicator.IsRunning = true;
            _activityIndicator.IsVisible = true;

            foreach (var i in _preferiti)
                _db.Insert(i);

            if (DateTime.Now.Hour < 18)
            {
                _oggi = new Giorno() { Data = DateTime.Today };
                _domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
                _dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
            }
            else
            {
                _oggi = new Giorno() { Data = DateTime.Today.AddDays(1) };
                _domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
                _dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
            }

            DateTime[] arrayDate = new DateTime[] { _oggi.Data, _domani.Data, _dopodomani.Data };
            foreach (var d in arrayDate)
            {
                //Corsi generale, utenza + corsi
                string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, 0, d.ToString("dd'/'MM'/'yyyy"));
                List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

                //CERCA SOLO TRA CORSI DI QUELLA LAUREA
                //string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, Settings.Laurea, d.ToString("dd'/'MM'/'yyyy"));
                //List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

                foreach (var l in listaCorsi)
                {
                    var corso = l;
                    if (_db.CheckAppartieneMieiCorsi(corso))
                    {
                        var orario = new Orari()
                        {
                            Insegnamento = corso.Insegnamento,
                            Codice = corso.Codice,
                            AulaOra = corso.AulaOra,
                            Note = corso.Note,
                            Date = corso.Date,
                            Docente = corso.Docente,
                            Notify = false,
                        };

                        _db.Insert(orario);
                    }
                    else if (corso.Insegnamento.Contains("Utenza"))
                        _db.Insert(new Utenza() { Data = corso.Date, Aulaora = corso.AulaOra });
                }

                //CERCA TRA I CORSI IN GENERALE, USO UTENZA
                //string s_ut = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, 0, d.ToString("dd'/'MM'/'yyyy"));
                //List<CorsoGiornaliero> listaUtenze = Web.GetSingleOrarioGiornaliero(s, 0, d);
                
                //foreach(var u in listaUtenze)
                //{
                //    var utenza = u;
                //    if (utenza.Insegnamento.Contains("Utenza"))
                //        _db.Insert(new Utenza() { Data = utenza.Date, Aulaora = utenza.AulaOra });
                //}
            }

            System.Diagnostics.Debug.WriteLine(_db.GetAllMieiCorsi().Count());
            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;
            await Navigation.PushModalAsync(new MasterDetailView());

        }
        #endregion

    }
}

