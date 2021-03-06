﻿using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Toasts;
using Plugin.Connectivity;
using OrariUnibg.Services.Azure;
using ImageCircle.Forms.Plugin.Abstractions;

namespace OrariUnibg.Views
{
    class InformationView : ContentPage
    {
        #region Constructor
        public InformationView()
		{
			NavigationPage.SetHasNavigationBar (this, true);
//			App.Current.MainPage = this;
            Title = "Informazioni";
            BackgroundColor = ColorHelper.White;
            Content = getView();
        }
        #endregion

        #region Properties
        public AzureDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }
        #endregion

        #region Private Fields
        AzureDataService _service;
        private CircleImage _imgAvatar;
        private Entry _entryNome;
        private Entry _entryCognome;
        private Entry _entryMail;
        private Entry _entryMatricola;
        private Picker _pickFacolta;
        private Picker _pickLaurea;
        private Picker _pickAnno;
        private Label _lblNotific;
        private Switch _switchNotific;
        private Label _lblSync;
        private Switch _switchSync;
		private ToolbarItem tbiNext;
        private int limit = 3;
        private ActivityIndicator _activityIndicator;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        #endregion

        #region Private Methods
        private View getView()
        {
            var lblWelcome = new Label() { Text = string.Format("Welcome {0}", Settings.Name), HorizontalOptions = LayoutOptions.CenterAndExpand };
            listFacolta = Facolta.facolta;

            _imgAvatar = new CircleImage
            {
                BorderColor = Color.White,
                BorderThickness = 1,
                HeightRequest = 150,
                WidthRequest = 150,
                Aspect = Aspect.AspectFill,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.StartAndExpand,
            };

            if (Settings.Picture != string.Empty)
                _imgAvatar.Source = Settings.Picture;
            //else //immagine predefinita o possibilità di caricare una propria foto????

            _entryNome = new Entry()
            {
                Placeholder = "Nome",
                //Text = "Lorenzo",
                HorizontalOptions = LayoutOptions.FillAndExpand,
				Keyboard = Keyboard.Text,
                Text = Settings.GivenName
            };
            _entryCognome = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                //Text = "Giudici",
                Placeholder = "Cognome",
				Keyboard = Keyboard.Text,
                Text = Settings.Surname
            };

            _entryCognome.TextChanged += _entryCognome_TextChanged;
            _entryMail = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Text = Settings.Username,
                //Placeholder = "n.cognome2"
            };
            _entryMatricola = new Entry()
            {
                Placeholder = "Matr.",
                //Text = "1020589",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            _pickFacolta = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Facoltà"
            };
            _pickLaurea = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Laurea",
                IsEnabled = false,
            };
            _pickAnno = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Anno",
                IsEnabled = false
            };

            foreach (var f in listFacolta)
            {
                _pickFacolta.Items.Add(f.Nome);
            }

            _pickFacolta.SelectedIndexChanged += (sender, args) =>
            {
                var s = _pickFacolta.Items[_pickFacolta.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                _pickLaurea.Items.Clear();

                foreach (var item in dictionaryLauree)
                    _pickLaurea.Items.Add(item.Key);

                _pickLaurea.SelectedIndex = 0;
                _pickLaurea.IsEnabled = true;
            };

            String[] anni = new String[] { "1° Anno", "2° Anno", "3° Anno", "4° Anno", "5° Anno" };
            _pickLaurea.SelectedIndexChanged += (sender, args) =>
            {
                int i = 1;
                _pickAnno.IsEnabled = true;

                if (_pickLaurea.Items.Count > 0)
                {
                    if (_pickLaurea.Items[_pickLaurea.SelectedIndex].Contains("Magistrale"))
                        limit = 5;
                    else if (_pickLaurea.Items[_pickLaurea.SelectedIndex].Contains("LM"))
                        limit = 2;
                    else limit = 3;

                    _pickAnno.Items.Clear();
                    foreach (var x in anni)
                    {
                        if (i > limit)
                            break;
                        _pickAnno.Items.Add(x);
                        i++;
                    }
                    _pickAnno.SelectedIndex = 0;
                }
            };

            _lblNotific = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Notifiche variazioni lezioni"
            };
            _switchSync = new Switch() { IsToggled = true};

            _lblSync = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Aggiornamento in bakgruound"
            };
            _switchNotific = new Switch() { IsToggled = true };

            _activityIndicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsRunning = true,
				IsVisible = false,
            };

            var grid = new Grid()
            {
                Padding = new Thickness(15, 5, 15, 5),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = 8,
                ColumnSpacing = 8,
                RowDefinitions = 
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions = 
                { 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) },
                }
            };

            //DA AGGIUNGERE ROW CON LABEL WELCOM NOME
            grid.Children.Add(_imgAvatar, 0, 2, 0, 1);
            grid.Children.Add(_entryNome, 0, 2, 1, 2);
            grid.Children.Add(_entryCognome, 0, 2, 2, 3);
            grid.Children.Add(_entryMail, 0, 1, 3, 4);
            grid.Children.Add(_entryMatricola, 1, 2, 3, 4);
            grid.Children.Add(_pickFacolta, 0, 2, 4, 5);
            grid.Children.Add(_pickLaurea, 0, 1, 5, 6);
            grid.Children.Add(_pickAnno, 1, 2, 5, 6);
            grid.Children.Add(_lblSync, 0, 1, 6, 7);
            grid.Children.Add(_switchSync, 1, 2, 6, 7);
            grid.Children.Add(_lblNotific, 0, 1, 7, 8);
            grid.Children.Add(_switchNotific, 1, 2, 7, 8);
            grid.Children.Add(_activityIndicator, 0, 2, 8, 9);

//            tbiNext = new ToolbarItem("Avanti", "ic_next.png", toolbarItem_next, 0, 0); 
//            //if (Device.OS == TargetPlatform.Android)
//            //{ // BUG: Android doesn't support the icon being null
//            //    tbiNext = new ToolbarItem("Avanti", "ic_menu.png", () =>
//            //    {
//            //        Navigation.PushAsync(new MasterDetailView());
//            //    }, 0, 0);
//            //}
//            //if (Device.OS == TargetPlatform.WinPhone)
//            //{
//            //    tbi = new ToolbarItem("Avanti", "ic_menu.png", () =>
//            //    {
//            //        Navigation.PushAsync(new MasterDetailView());
//            //    }, 0, 0);
//            //}
//            ToolbarItems.Add(tbiNext);
            return grid;
        }

		private async void CheckNetwork()
		{
			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				var toast = DependencyService.Get<IToastNotificator>();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				return;
			}
		}
        #endregion

        #region Event Handlers
//		protected override bool OnBackButtonPressed ()
//		{
//			return true;
////			return base.OnBackButtonPressed ();
//		}
		protected override void OnAppearing()
		{
			tbiNext = new ToolbarItem("Avanti", "ic_next.png", toolbarItem_next, 0, 0); 
			ToolbarItems.Add(tbiNext);
		}
        private async void toolbarItem_next()
        {

            if (_entryNome.Text == string.Empty || _entryCognome.Text == string.Empty || _entryMatricola.Text == string.Empty || _pickFacolta.SelectedIndex < 0)
            {
                await DisplayAlert("ATTENZIONE", "Occorre compilare tutti i campi correttmente prima di procedere.", "OK");
                return;
            }

			CheckNetwork ();

			ToolbarItems.Remove (tbiNext);
            _activityIndicator.IsVisible = true;

            Facolta fac = listFacolta.Where(x => x.Nome == _pickFacolta.Items[_pickFacolta.SelectedIndex]).First();
            int facoltaId = fac.IdFacolta;
            int facoltaIndex = _pickFacolta.SelectedIndex;
            string facolta = fac.Nome;
            string db = fac.DB;
            int laureaId = dictionaryLauree.Where(x => x.Key == _pickLaurea.Items[_pickLaurea.SelectedIndex]).First().Value;
            int laureaIndex = _pickLaurea.SelectedIndex;
            string laurea = _pickLaurea.Items[_pickLaurea.SelectedIndex];
            int annoIndex = _pickAnno.SelectedIndex + 1;
            string anno = _pickAnno.Items[annoIndex - 1];
			Settings.Name = _entryNome.Text.TrimEnd();
			Settings.GivenName = _entryCognome.Text.TrimEnd();
			Settings.Email = _entryMail.Text.Trim () + "@studenti.unibg.it";

            
            Settings.Facolta = facolta;
            Settings.FacoltaDB = db;
            Settings.Laurea = laurea;
            Settings.Anno = anno;
            Settings.BackgroundSync = _switchSync.IsToggled;
            Settings.Notify = _switchNotific.IsToggled;

            Settings.Matricola = _entryMatricola.Text;
            Settings.FacoltaId = facoltaId;
            Settings.LaureaId = laureaId;
            Settings.LaureaIndex = laureaIndex;
            Settings.FacoltaIndex = facoltaIndex;
            Settings.AnnoIndex = annoIndex;

            Settings.CreatedAtString = _service.User.CreatedAt.Date.ToString("d");
            Settings.UpdatedAtString = _service.User.CreatedAt.Date.ToString("d");

            _service.User.BackgroundSync = Settings.BackgroundSync;

            //aggiornamento User
            _service.User.Matricola = _entryMatricola.Text;
            _service.User.FacoltaDB = db;
            _service.User.FacoltaId = facoltaId;
            _service.User.LaureaId = laureaId;
            _service.User.AnnoIndex = annoIndex;

            await _service.UpdateUser(); 

            if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				var toast = DependencyService.Get<IToastNotificator> ();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				ToolbarItems.Add (tbiNext);
				return;
			}

            string completo = await Web.GetOrarioCompleto("completo", db, facoltaId, laureaId, annoIndex);
            string secondo = await Web.GetOrarioCompleto("secondo", db, facoltaId, laureaId, annoIndex);

            List<CorsoCompleto> lista_completo = Web.GetSingleOrarioCompleto(completo);
            List<CorsoCompleto> lista_secondo = Web.GetSingleOrarioCompleto(secondo);

			if(lista_completo.Count() != 0 && lista_completo.FirstOrDefault().Semestre == 1)
                lista_completo.AddRange(lista_secondo);

//            _activityIndicator.IsVisible = false;

            Settings.PrimoAvvio = false;
			Settings.SuccessLogin = true;

			//var nav = new NavigationPage (new ListaCorsi(lista_completo) { Service = _service}) {
			//	BarBackgroundColor = ColorHelper.Blue700,
			//	BarTextColor = ColorHelper.White
			//};

            await Navigation.PushAsync(new ListaCorsi(lista_completo) { Service = _service });
        }
        
        void _entryCognome_TextChanged(object sender, TextChangedEventArgs e)
        {
            var s = (Entry)sender;
            string cognome = "";
            char nome = ' ';
            if (_entryNome.Text != string.Empty)
                nome = _entryNome.Text[0];
			
            if (s.Text != string.Empty)
                cognome = s.Text;

            if (_entryNome.Text != string.Empty && _entryCognome.Text != string.Empty)
                _entryMail.Text = string.Format("{0}.{1}", nome.ToString().ToLower(), cognome.ToLower());
        }

        #endregion
    }
}
