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
        private DateTime _dateOggi;
        private DateTime _dateDomani;
        private DateTime _dateDopodomani;
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

            ToolbarItem tbiNext = new ToolbarItem("Avanti", "ic_next.png", async () =>
            {
                foreach(var i in _preferiti)
                {
                    _db.Insert(i);
                    
                }

                if (DateTime.Now.Hour < 18)
                {
                    _dateOggi = DateTime.Today;
                    _dateDomani = DateTime.Today.AddDays(1);
                    _dateDopodomani = DateTime.Today.AddDays(2);
                }
                else
                {
                    _dateOggi = DateTime.Today.AddDays(1);
                    _dateDomani = DateTime.Today.AddDays(2);
                    _dateDopodomani = DateTime.Today.AddDays(3);
                }

                DateTime[] arrayDate = new DateTime[]{_dateOggi, _dateDomani, _dateDopodomani };
                foreach (var d in arrayDate)
                {
                    string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, Settings.Laurea, d.ToString("dd'/'MM'/'yyyy"));
                    List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

                    foreach (var l in listaCorsi)
                    {
                        if (_db.CheckAppartieneMieiCorsi(l))
                        {
                            _db.InsertUpdate(l);
                        }
                    }
                }

                System.Diagnostics.Debug.WriteLine(_db.GetItemsMieiCorsi().Count());
                await Navigation.PushModalAsync(new MasterDetailView());

            }, 0, 0);

            ToolbarItems.Add(tbiNext);
            return _list;
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
    }
}
