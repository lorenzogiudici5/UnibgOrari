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
            //BackgroundColor = ColorHelper.White;
            _listSource = lista_completo;
            Content = getView();
        }
        #endregion

        #region Private Fields
        private List<CorsoCompleto> _listSource;
        private List<MieiCorsi> _preferiti;
        private ListView _list;
        private DbSQLite _db;
        private ActivityIndicator _activityIndicator;
        private Giorno _oggi;
        private Giorno _domani;
        private Giorno _dopodomani;
		private ToolbarItem tbiNext;
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

            tbiNext = new ToolbarItem("Avanti", "ic_next.png", toolbarItem_next, 0, 0);

            ToolbarItems.Add(tbiNext);

            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
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
			ToolbarItems.Remove (tbiNext);
            _activityIndicator.IsRunning = true;
            _activityIndicator.IsVisible = true;


            foreach (var i in _preferiti)
                _db.Insert(i);
            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();

			if (DateTime.Now.Hour > Settings.UpdateHour) // && DateTime.Now.Minute > Settings.UpdateMinute)
			{
				_oggi = new Giorno() { Data = DateTime.Today.AddDays(1) };
				_domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
			}
			else
			{
				_oggi = new Giorno() { Data = DateTime.Today };
				_domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
			}

            DateTime[] arrayDate = new DateTime[] { _oggi.Data, _domani.Data, _dopodomani.Data };
            foreach (var d in arrayDate)
            {
                //Corsi generale, utenza + corsi
                string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.FacoltaId, 0, d.ToString("dd'/'MM'/'yyyy"));
                List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

                //CERCA SOLO TRA CORSI DI QUELLA LAUREA
                //string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, Settings.Laurea, d.ToString("dd'/'MM'/'yyyy"));
                //List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

				TabbedHomeView.updateSingleCorso (_db, listaCorsi);
				var orariXX = _db.GetAllOrari ();
//                foreach (var l in listaCorsi)
//                {
//                    var corso = l;
//                    if (_db.CheckAppartieneMieiCorsi(corso))
//                    {
//                        var orario = new Orari()
//                        {
//                            Insegnamento = corso.Insegnamento,
//                            Codice = corso.Codice,
//                            AulaOra = corso.AulaOra,
//                            Note = corso.Note,
//							Date = corso.Date.Date,
//                            Docente = corso.Docente,
//                            Notify = false,
//                        };
//
//                        _db.Insert(orario);
//                    }
//                    else if (corso.Insegnamento.Contains("UTENZA"))
//                        _db.Insert(new Utenza() { Data = corso.Date, AulaOra = corso.AulaOra });
//                }
            }

			Logcat.Write(_db.GetAllMieiCorsi().Count());
			var orariX = _db.GetAllOrari ();

            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;


			if(Settings.BackgroundSync)
				DependencyService.Get<INotification> ().BackgroundSync ();
			
			await Navigation.PushModalAsync(new MasterDetailView());

        }
        #endregion

    }
}

