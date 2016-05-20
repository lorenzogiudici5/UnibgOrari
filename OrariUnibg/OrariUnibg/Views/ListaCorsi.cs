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
using Plugin.Connectivity;
using Plugin.Toasts;
using OrariUnibg.Services.Azure;
using OrariUnibg.ViewModels;

namespace OrariUnibg.Views
{
    class ListaCorsi : ContentPage
    {
        #region Constructor
        public ListaCorsi(List<CorsoCompleto> lista_completo)
        {
            Title = "Corsi";
            _db = new DbSQLite();
            //BackgroundColor = ColorHelper.White;
            _listSource = lista_completo;
            Content = getView();
        }
        #endregion

        #region Private Fields
        private AzureDataService _service;
        private Label _lblInfo;
        private List<CorsoCompleto> _listSource;
        private List<Preferiti> _preferiti;
        private ListView _list;
        private DbSQLite _db;
        private ActivityIndicator _activityIndicator;
        private DayViewModel _oggi;
        private DayViewModel _domani;
        private DayViewModel _dopodomani;
		private ToolbarItem tbiNext;
        #endregion

        #region Properties
        public AzureDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }
        #endregion


        #region Private Methods
        private View getView()
        {
			_lblInfo = new Label () {
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				Text = "Seleziona i tuoi corsi",
				TextColor = ColorHelper.Green500,
				HorizontalOptions = LayoutOptions.CenterAndExpand,
			};

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
            _preferiti = new List<Preferiti>();

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
                Children = { _lblInfo, _list, _activityIndicator}
            };
            return layout;
        }

        
        #endregion

        #region Event Handlers
		void _list_ItemSelected(object sender, SelectedItemChangedEventArgs e)
		{
			if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
				return;
			var selected = (CorsoCompleto)_list.SelectedItem;

			Preferiti newFavourite = new Preferiti() { Codice = selected.Codice, Insegnamento = selected.Insegnamento, Docente = selected.Docente };

			int index = _preferiti.FindIndex(f => f.Insegnamento == newFavourite.Insegnamento);
			if (index >= 0)
			{
				MessagingCenter.Send<ListaCorsi, Preferiti>(this, "deselect_fav", newFavourite);
				_preferiti.RemoveAt(index);
			}
			else
			{
				MessagingCenter.Send<ListaCorsi, Preferiti>(this, "select_fav", newFavourite);
				_preferiti.Add(newFavourite);
			}

			((ListView)sender).SelectedItem = null;
		}

        private async void toolbarItem_next()
        {
			ToolbarItems.Remove (tbiNext);
            _activityIndicator.IsRunning = true;
            _activityIndicator.IsVisible = true;

            //**NON C'E CONNESSIONE INTERNET**
            if (!CrossConnectivity.Current.IsConnected)
            {   //non connesso a internet
                var toast = DependencyService.Get<IToastNotificator>();
                await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                return;
            }

            foreach (var preferito in _preferiti)
            {
                //add corso 
                var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };
                await _service.AddCorso(corso);
                corso = await _service.GetCorso(corso);
                preferito.IdCorso = corso.Id;
                //add preferito
                await _service.AddPreferito(preferito);

                _db.Insert(preferito);
            }

			
            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();

			if (DateTime.Now.Hour > Settings.UpdateHour) // && DateTime.Now.Minute > Settings.UpdateMinute)
			{
				_oggi = new DayViewModel() { Data = DateTime.Today.AddDays(1) };
				_domani = new DayViewModel() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new DayViewModel() { Data = _domani.Data.AddDays(1) };
			}
			else
			{
				_oggi = new DayViewModel() { Data = DateTime.Today };
				_domani = new DayViewModel() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new DayViewModel() { Data = _domani.Data.AddDays(1) };
			}

            DateTime[] arrayDate = new DateTime[] { _oggi.Data, _domani.Data, _dopodomani.Data };

			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				var toast = DependencyService.Get<IToastNotificator>();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
			}else { //CONNESSIONE A INTERNET
				foreach (var d in arrayDate) {

					//Corsi generale, utenza + corsi
					string s = await Web.GetOrarioGiornaliero (Settings.FacoltaDB, Settings.FacoltaId, 0, d.ToString ("dd'/'MM'/'yyyy"));
					List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero (s, 0, d);

					if (listaCorsi.Count () != 0)
						TabbedHomeView.updateSingleCorso (_db, listaCorsi);

				}
			}

            _activityIndicator.IsRunning = false;
            _activityIndicator.IsVisible = false;

			if(Settings.BackgroundSync)
				DependencyService.Get<INotification> ().BackgroundSync ();
			
			await Navigation.PushModalAsync(new MasterDetailView() { Service = _service });
        }
        #endregion

    }
}

