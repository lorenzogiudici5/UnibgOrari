using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using System.Linq;
using System.Threading.Tasks;
using OrariUnibg.Views;
using OrariUnibg.Services.Authentication;

namespace OrariUnibg
{
	public class ImpostazioniView : ContentPage
	{
		#region Constructor
		public ImpostazioniView()
		{
			Title = "Impostazioni";
			Icon = null;
			_db = new DbSQLite ();
			Content = getView();
		}
		#endregion

		#region Private Fields
		private DbSQLite _db;
		private Label _lblBackgroundSync;
		private ViewCell _backgroundSyncCell;
		private Switch _backgroundSyncSwitch;
		private Label _lblNotifiche;
		private ViewCell _notificheCell;
		private Switch _notificheSwitch;
		private Label _lblInterval;
		private ViewCell _updateIntervallCell;
		private Label _lblLastUpdate;
		private ViewCell _lastUpdateCell;
		private ViewCell _corsiPreferitiCell;
		private Label _lblCorsiPreferiti;
		private ViewCell _fileCell;
		private Label _lblFile;
		private int fileCount;
		private Label _lblStatistic;
		private ViewCell _statisticCell;
		private Switch _statisticSwitch;
		private ViewCell _versionCell;
        private ViewCell _logCell;
        private ViewCell _tutorialCell;
		private ViewCell _logoutCell;
		#endregion

		#region Private Methods
		private View getView()
		{
			var table = new TableView () {
				Intent = TableIntent.Settings,
				HasUnevenRows = true,
				BackgroundColor = ColorHelper.White,
			};

			#region IntervalUpdate
			_lblInterval = new Label () { Text = getIntervalString(), TextColor = ColorHelper.DarkGray };
			var _updateIntervallLayout = new StackLayout () {
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = 
				{
					new Label(){Text = "Intervallo di aggiornamento", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
					_lblInterval
				}					
				};
			_updateIntervallCell = new ViewCell (){ View = _updateIntervallLayout, IsEnabled = Settings.BackgroundSync};
			_updateIntervallCell.Tapped += _updateIntervallCell_Tapped;

			#endregion


			#region AggiornamentoBackground
			_backgroundSyncSwitch = new Switch() {IsToggled = Settings.BackgroundSync, HorizontalOptions = LayoutOptions.EndAndExpand};
			_lblBackgroundSync = new Label () { Text = getBackgroundSyncString(), TextColor = ColorHelper.DarkGray };
			var _backgroundSyncLayout = new Grid () {
				Padding = new Thickness(20, 10, 20, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,

				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{ 
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
			};
			_backgroundSyncLayout.Children.Add(new Label(){Text = "Aggiornamento background", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"}, 0, 1, 0, 1);
			_backgroundSyncLayout.Children.Add(_backgroundSyncSwitch, 1,2,0,1);
			_backgroundSyncLayout.Children.Add(_lblBackgroundSync, 0, 1, 1, 2);

			_backgroundSyncCell = new ViewCell (){ View = _backgroundSyncLayout};
			_backgroundSyncSwitch.Toggled += (object sender, ToggledEventArgs e) =>
			{
				if (_backgroundSyncSwitch.IsToggled) {
					Settings.BackgroundSync = true;
					_updateIntervallCell.IsEnabled = true;

				} else {
					Settings.BackgroundSync = false;
					_updateIntervallCell.IsEnabled = false;
				}
				_lblBackgroundSync.Text = getBackgroundSyncString();
			};
			#endregion

			#region Notifiche
			_notificheSwitch = new Switch() {IsToggled = Settings.Notify, HorizontalOptions = LayoutOptions.EndAndExpand};
			_lblNotifiche = new Label () { Text = getNotificheString(), TextColor = ColorHelper.DarkGray };
			var _notificheLayout = new Grid () {
				Padding = new Thickness(20, 10, 20, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,

				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{ 
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
				};
			_notificheLayout.Children.Add(new Label(){Text = "Notifiche", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"}, 0, 1, 0, 1);
			_notificheLayout.Children.Add(_notificheSwitch, 1,2,0,1);
			_notificheLayout.Children.Add(_lblNotifiche, 0, 1, 1, 2);

			_notificheCell = new ViewCell (){ View = _notificheLayout};
			_notificheSwitch.Toggled += (object sender, ToggledEventArgs e) =>
			{
				if (_notificheSwitch.IsToggled) {
					Settings.Notify = true;
				} else {
					Settings.Notify = false;
				}
				_lblNotifiche.Text = getNotificheString();
			};
			#endregion


			#region LastUpdate
			_lblLastUpdate = new Label () { Text = Settings.LastUpdate, TextColor = ColorHelper.DarkGray };
			var _lastUpdatelLayout = new StackLayout () {
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = 
				{
					new Label(){Text = "Ultimo Aggiornamento", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
					_lblLastUpdate
				}					
				};
			
			_lastUpdateCell = new ViewCell (){ View = _lastUpdatelLayout};
			_lastUpdateCell.IsEnabled = false;
			#endregion

			var sectionSync = new TableSection ("Sincronizzazione") { //TableSection constructor takes title as an optional parameter
				_backgroundSyncCell,
				_notificheCell,
				_updateIntervallCell,
				_lastUpdateCell,
			};

			#region Corsi Preferiti
			_lblCorsiPreferiti = new Label () {TextColor = ColorHelper.DarkGray };
			getPreferitiString();
			var _corsiPrefLayout = new StackLayout()
			{
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = {
					new Label(){Text = "Corsi Preferiti", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
					_lblCorsiPreferiti
				}
			};
			_corsiPreferitiCell = new ViewCell (){ View = _corsiPrefLayout};
			_corsiPreferitiCell.Tapped += async (object sender, EventArgs e) => 
			{
				var nav = new GestionePreferitiView();
//				nav.BindingContext = orariViewModel;
				await this.Navigation.PushAsync(nav);
			};
			#endregion

			#region Manage File
			_lblFile = new Label () { TextColor = ColorHelper.DarkGray };
			getFileString();

			var _fileLayout = new StackLayout()
			{
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = {
					new Label(){Text = "File generati", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
					_lblFile
				}
			};
			_fileCell = new ViewCell () { View = _fileLayout};
			_fileCell.Tapped += async (object sender, EventArgs e) => 
			{
				if(fileCount > 0)
				{
					var nav = new ManageFileView();
					await this.Navigation.PushAsync(nav);
				}

			};
			#endregion

			var sectionFavourit = new TableSection ("Gestione") { //TableSection constructor takes title as an optional parameter
				_corsiPreferitiCell,
				_fileCell
			};



			#region DatiStatistici
			_statisticSwitch = new Switch() {IsToggled = !Settings.DisableStatisticData, HorizontalOptions = LayoutOptions.EndAndExpand};
			_lblStatistic = new Label () { Text = getStatisticString(), TextColor = ColorHelper.DarkGray };
			var _statistcLayout = new Grid () {
				Padding = new Thickness(20, 10, 20, 10),
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,

				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{ 
					new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
				};
			_statistcLayout.Children.Add(new Label(){Text = "Dati Statistici", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"}, 0, 1, 0, 1);
			_statistcLayout.Children.Add(_statisticSwitch, 1,2,0,1);
			_statistcLayout.Children.Add(_lblStatistic, 0, 1, 1, 2);

			_statisticCell = new ViewCell (){ View = _statistcLayout};
			_statisticSwitch.Toggled += (object sender, ToggledEventArgs e) =>
			{
				if (_statisticSwitch.IsToggled) {
					Settings.DisableStatisticData = false;
				} else {
					Settings.DisableStatisticData = true;
				}

				_lblStatistic.Text = getStatisticString();
			};
			#endregion

			#region Tutorial
			var _tutorialLayout = new StackLayout () 
			{
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = 
				{
					new Label() {Text = "Tutorial", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
				}					
				};

			_tutorialCell = new ViewCell (){View = _tutorialLayout};
			_tutorialCell.Tapped += async (object sender, EventArgs e) => await Navigation.PushModalAsync(new TutorialView());
			#endregion

			#region Version
			var _versionLayout = new StackLayout () 
			{
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = 
				{
					new Label() {Text = "Versione", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
					new Label () {Text = Settings.Versione, TextColor = ColorHelper.DarkGray}
				}					
			};
			_versionCell = new ViewCell () {View = _versionLayout}; //, IsEnabled = false };
			_versionCell.Tapped += async (object sender, EventArgs e) => await Navigation.PushModalAsync(new AboutView());
            #endregion

            #region Log
            var _logLayout = new StackLayout()
            {
                Padding = new Thickness(20, 10, 20, 10),
                Orientation = StackOrientation.Vertical,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 3,
                Children =
                {
                    new Label() {Text = "Log", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
                }
            };
            _logCell = new ViewCell() { View = _logLayout }; //, IsEnabled = false };
            _logCell.Tapped += async (object sender, EventArgs e) =>
            {
                var nav = new LogView();
                await this.Navigation.PushAsync(nav);
            };
            #endregion

            #region Logout
            var _logoutLayout = new StackLayout () 
			{
				Padding = new Thickness(20, 10, 20, 10),
				Orientation = StackOrientation.Vertical,
				VerticalOptions = LayoutOptions.FillAndExpand,
				Spacing = 3,
				Children = 
				{
					new Label() {Text = "Esci", TextColor = ColorHelper.Black, FontFamily = "Droid Sans Mono"},
				}					
			};

			_logoutCell = new ViewCell (){View = _logoutLayout};
			_logoutCell.Tapped += _logoutCell_Tapped;
			#endregion



			var sectionInfo = new TableSection ("Informazioni") {
				_statisticCell,
				_tutorialCell,
				_versionCell,
                _logCell,
				_logoutCell
			};
				
			table.Root = new TableRoot () {
				sectionSync,
				sectionFavourit,
				sectionInfo
			};

			return table;
		}

		private void getPreferitiString(){
			if(Settings.MieiCorsiCount == 1)
				_lblCorsiPreferiti.Text = String.Format ("{0} corso", Settings.MieiCorsiCount);
			else
				_lblCorsiPreferiti.Text = String.Format ("{0} corsi", Settings.MieiCorsiCount);
		}

		private async void getFileString(){
			
			var path = await DependencyService.Get<IFile> ().GetInternalFolder ();
			var files = DependencyService.Get<IFile> ().GetFiles (path); //.Count();
			fileCount = files.Count();
			if(fileCount == 1)
				_lblFile.Text = String.Format ("{0} file", fileCount);
			else
				_lblFile.Text = String.Format ("{0} files", fileCount);
		}

		private string getNotificheString()
		{
			if (Settings.Notify) {
				return "Attive";
			} else {
				return "Non attive";
			}
		}

		private string getStatisticString()
		{
			if (!Settings.DisableStatisticData) {
				return "Raccolta dati statici attivata";
			} else {
				return "Raccolta dati statici disattivata";
			}
		}

		private string getBackgroundSyncString()
		{
			if (Settings.BackgroundSync) {
				return "Attivo";
			} else {
				return "Non attivo";
			}
		}

		private string getIntervalString(){
			if(Settings.UpdateInterval == 1)
				return String.Format ("{0} ora", Settings.UpdateInterval);
			else
				return String.Format ("{0} ore", Settings.UpdateInterval);
		}
		#endregion



		#region EventHandlers
		protected override bool OnBackButtonPressed ()
		{
			return true; //ALERT CHIUDERE APP??
		}
		async void _updateIntervallCell_Tapped (object sender, EventArgs e)
		{
			//var interval = await DisplayActionSheet ("Scegli intervallo di aggiornamento", "Annulla", null, "30 minuti", "1 ora", "3 ore", "6 ore", "12 ore", "24 ore");
            var interval = await DisplayActionSheet("Scegli intervallo di aggiornamento", "Annulla", null, "1 ora", "3 ore", "6 ore", "12 ore", "24 ore");
            switch (interval) {
            //case "30 minuti": //*** SOLO TEST!! ELIMINARE
            //    Settings.UpdateInterval = (long) 0.5F;
            //    break;
            case "1 ora":
				Settings.UpdateInterval = 1;
				break;
			case "3 ore":
				Settings.UpdateInterval = 3;
				break;
			case "6 ore":
				Settings.UpdateInterval = 6;
				break;
			case "12 ore":
				Settings.UpdateInterval = 12;
				break;
			case "24 ore":
				Settings.UpdateInterval = 24;
				break;
			}

			_lblInterval.Text = getIntervalString ();
		}

		async void _logoutCell_Tapped (object sender, EventArgs e)
		{
			Settings.SuccessLogin = false;
            DependencyService.Get<IAuthentication>().ClearCookies();
            //reset Settings!
            Settings.UserId = string.Empty; //cosi diventa false Settings.IsLoggedIn
            //.. . . 
            await Navigation.PushModalAsync(new LoginView());
		}
		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			Settings.MieiCorsiCount = _db.GetAllMieiCorsi ().Count();
			getPreferitiString ();
			_lblLastUpdate.Text = Settings.LastUpdate;

			getFileString ();
		}
		#endregion
	}
}

