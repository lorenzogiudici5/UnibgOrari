using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Views;
using OrariUnibg.Services.Azure;
using OrariUnibg.Services.Authentication;
using Microsoft.WindowsAzure.MobileServices;
using System.Threading.Tasks;
using OrariUnibg.Services.Database;
using OrariUnibg.Services;
using Plugin.Connectivity;
using Plugin.Toasts;
using Newtonsoft.Json.Linq;
using System.Collections.Generic;
using System.Net.Http;

namespace OrariUnibg
{
	public class LoginView : ContentPage
	{
		#region Constructor
		public LoginView ()
		{
            //_service = new AzureDataService();
            //_db = new DbSQLite();
            _service = new AzureDataService();
            Content = getView ();
		}
        #endregion

        #region Property
        public AzureDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }
        #endregion

        #region Private Fields
        private AzureDataService _service;
        private DbSQLite _db;
        private Button _btnAccedi;
        private Label _lblSalta;
        private Label _lblAlert;
        private ActivityIndicator _activityIndicator;
        #endregion

        #region Private Methods
        private View getView()
		{
            var _btnNext = new Button () {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "                Fine                ",
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.Black,
                BackgroundColor = ColorHelper.White,
            };
			_btnNext.Clicked += _btnNext_Clicked;

            _activityIndicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                IsRunning = true,
                IsVisible = false
            };
            _lblAlert = new Label()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                TextColor = ColorHelper.White,
                FontSize = 25,
                IsVisible = true,
            };

            var layout = new StackLayout () {
				BackgroundColor = ColorHelper.Blue700,
				VerticalOptions = LayoutOptions.FillAndExpand,
				HorizontalOptions = LayoutOptions.FillAndExpand,
				Padding = new Thickness (0, 15, 0, 40),
				Children = {
					new Image () {
						HorizontalOptions = LayoutOptions.CenterAndExpand,
						Source = "splashFlat.png", 
					},
				}
			};

			if (Settings.PrimoAvvio || !Settings.IsLoggedIn) //se tutorial da primo avvio aggiungo button per signin
            {
                layout.Children.Add(_lblAlert);
                layout.Children.Add(_activityIndicator);
                layout.Children.Add(getLayoutButtons());
            }

			else //altrimenti è tutorial da impostazioni
				layout.Children.Add (_btnNext);

			Settings.PrimoAvvio = false;

			return layout;
		}

		private StackLayout getLayoutButtons()
		{
            _btnAccedi = new Button () {
				HorizontalOptions = LayoutOptions.CenterAndExpand,
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "                 Accedi                 ",
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				TextColor = ColorHelper.Black,
				BackgroundColor = ColorHelper.White,
//				BackgroundColor = ColorHelper.LightBlue500,
			};
			_btnAccedi.Clicked += _btnAccedi_Clicked;

            _lblSalta = new Label()
            {
                Text = "Salta>>",
                TextColor = ColorHelper.White,
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                HorizontalOptions = LayoutOptions.EndAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            var salta_tap = new TapGestureRecognizer();
            salta_tap.Tapped += _btnSalta_Clicked;
            _lblSalta.GestureRecognizers.Add(salta_tap);

            var layout = new StackLayout() {
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.EndAndExpand,
                Padding = new Thickness(20, 0, 20, 0),
				Spacing = 20, 
				Children =
                {
                    //_lblAlert,
                    _btnAccedi,
                    //_activityIndicator,
                    //_btnRegister,
                    _lblSalta  }
			};


			return layout;

		}

        private async Task<bool> addUser()
        {
            try
            {
                return await _service.AddUser();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("ECCEZIONE: " + ex.ToString());
                return true;
            }
        }

        private bool checkUniversityInformation()
        {
            if (_service.User.Matricola == string.Empty ||
                _service.User.LaureaId == null ||
                _service.User.FacoltaId == null ||
                _service.User.FacoltaDB == string.Empty ||
                _service.User.AnnoIndex == null)
                return false;

            else return true;
        }
        #endregion

        #region Event Handler
        protected override bool OnBackButtonPressed ()
		{
			DependencyService.Get<IMethods>().Close_App(); //altrmenti nulla
			return true; //ALERT CHIUDERE APP??
		}
		void _btnRegister_Clicked (object sender, EventArgs e)
		{
			var nav = new NavigationPage (
				new InformationView ()) 
			{
				BarBackgroundColor = ColorHelper.Blue700,
				BarTextColor = ColorHelper.White
			};
			Navigation.PushModalAsync (nav);
		}

		private async void _btnAccedi_Clicked (object sender, EventArgs e)
		{
            await _service.Initialize();
            _btnAccedi.IsVisible = false;
            _lblSalta.IsVisible = false;
            _lblAlert.IsVisible = true;
            _activityIndicator.IsVisible = true;



            _lblAlert.Text = string.Format("Accesso in corso");
            var user = await DependencyService.Get<IAuthentication>().LoginAsync(_service.MobileService, MobileServiceAuthenticationProvider.Google);
            _lblAlert.Text = string.Format("Sto ottenendo le tue informazioni");

            //aggiunge utente alla tabella
            var isNewUser = await addUser();

            //**NELL'IMPLEMENTAZIONE Dell'authentication
            //Settings.Email = user.Message.Email;
            //Settings.Username = Settings.Email.Split('@')[0];
            //Settings.Name = user.Message.Name;
            //Settings.SocialId = user.Message.SocialId;

            var name = Settings.Name;

            NavigationPage nav;

            //if informazioni facoltà, laurea, anno NON sono già presenti -> è il primo accesso -> information view
            //if (true) ///*********************************SOLO PER PROVA DEBUG TEST AZURE
            if (isNewUser || !checkUniversityInformation())
            {
                nav = new NavigationPage(new InformationView() { Service = _service })
                {
                    BarBackgroundColor = ColorHelper.Blue700,
                    BarTextColor = ColorHelper.White,
                };
                Settings.SuccessLogin = true;
                await Navigation.PushModalAsync(nav);
            }
            else //altrimenti accedo direttamente
            {
                _lblAlert.Text = string.Format("Sto sincronizzando i tuoi dati");
                _db = new DbSQLite();

                var toast = DependencyService.Get<IToastNotificator>();
                if (!CrossConnectivity.Current.IsConnected)
                { //non connesso a internet
                    await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                    return;
                }

                var success = await _db.SynchronizeAzureDb();

                if (!success)
                    await toast.Notify(ToastNotificationType.Error, "Errore", "Sincronizzazione fallita!", TimeSpan.FromSeconds(3));

                Settings.SuccessLogin = true;
                Settings.ToUpdate = true;

                if(Settings.BackgroundSync)
                    DependencyService.Get<INotification>().BackgroundSync();

                await Navigation.PushModalAsync(new MasterDetailView());
            }
        }

		async void _btnSalta_Clicked (object sender, EventArgs e)
		{
			var nav = new MasterDetailView ();
			//			var nav = new NavigationPage (
			//				new MasterDetailView ()) 
			//				{
			//					BarBackgroundColor = ColorHelper.Blue700,
			//					BarTextColor = ColorHelper.White
			//				};
			await Navigation.PushModalAsync (nav);
		}

		void _btnNext_Clicked (object sender, EventArgs e)
		{
			Navigation.PopModalAsync ();
		}
		#endregion
	}
}

