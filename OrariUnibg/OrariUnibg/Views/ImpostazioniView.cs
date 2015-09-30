using System;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;

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
		private DbSQLite _db;
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
			_lblCorsiPreferiti = new Label () { Text = getPreferitiString(), TextColor = ColorHelper.DarkGray };
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

			var sectionFavourit = new TableSection ("Gestione Preferiti") { //TableSection constructor takes title as an optional parameter
				_corsiPreferitiCell
			};
				
			table.Root = new TableRoot () {
				sectionSync,
				sectionFavourit
			};
				
			return table;
		}

		private string getPreferitiString(){
			if(Settings.MieiCorsiCount == 1)
				return String.Format ("{0} corso", Settings.MieiCorsiCount);
			else
				return String.Format ("{0} corsi", Settings.MieiCorsiCount);
		}

		private string getNotificheString()
		{
			if (Settings.Notify) {
				return "Attive";
			} else {
				return "Non attive";
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
		async void _updateIntervallCell_Tapped (object sender, EventArgs e)
		{
			var interval = await DisplayActionSheet ("Scegli intervallo di aggiornamento", "Annulla", null, "1 ora", "3 ore", "6 ore", "12 ore", "24 ore");
			switch (interval) {
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

		protected override void OnAppearing ()
		{
			base.OnAppearing ();
			Settings.MieiCorsiCount = _db.GetAllMieiCorsi ();
			_lblCorsiPreferiti.Text = getPreferitiString ();
			_lblLastUpdate.Text = Settings.LastUpdate;
		}
		#endregion
	}
}

