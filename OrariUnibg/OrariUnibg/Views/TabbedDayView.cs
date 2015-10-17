using OrariUnibg.Helpers;
using OrariUnibg.Models;
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
    public class TabbedDayView : ContentPage
    {
        #region Constructor
        public TabbedDayView()
        {
            _db = new DbSQLite();
            this.SetBinding(ContentPage.TitleProperty, "Day");
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView _listView;
//        private Label _lblDay;
//        private Label _lblDate;
        private Label _lblInfo;
        private Label _lblTitleUtenza;
        private Label _lblUtenza;
        private ActivityIndicator _activityIndicator;
        private DbSQLite _db;
        private ListView _listUtenze;
		private StackLayout layoutListaUtenza;
		private Giorno _viewModel;
        #endregion

        #region Private Methods
        private View getView()
        {
			var fab = new FloatingActionButtonView() {
				ImageName = "ic_sharee.png",
				ColorNormal = ColorHelper.Blue500,
				ColorPressed = ColorHelper.Blue900,
				ColorRipple = ColorHelper.Blue500,
				Size = FloatingActionButtonSize.Normal,
				Clicked = (sender, args) => 
				{
					string text = _viewModel.ToString () + Settings.Firma;
					DependencyService.Get<IMethods> ().Share (text);
				}
			};

//            _lblDay = new Label()
//            {
//                FontSize = Device.GetNamedSize(NamedSize.Small, this),
//                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
//                HorizontalOptions = LayoutOptions.EndAndExpand,
//            };
//            _lblDay.SetBinding(Label.TextProperty, "Day");
//
//            _lblDate = new Label()
//            {
//                FontSize = Device.GetNamedSize(NamedSize.Small, this),
//                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
//                HorizontalOptions = LayoutOptions.StartAndExpand,
//            };
//            _lblDate.SetBinding(Label.TextProperty, "DateString");

            _lblInfo = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Rilassati! Non hai lezioni!",
				TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            _lblInfo.SetBinding(Label.IsVisibleProperty, new Binding("ListaLezioni", converter: new IsVisibleCountConverter()));


		
            _listView = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioFavCell)),
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorColor = Color.Transparent,
                HasUnevenRows = true,
            };
            _listView.SetBinding(ListView.ItemsSourceProperty, "ListaLezioni");
			_listView.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};
//			_listView.ItemSelected += _listView_ItemSelected;

			_listUtenze = new ListView()
			{
				ItemTemplate = new DataTemplate(typeof(UtenzaCell)),
				HasUnevenRows = true,
				//				VerticalOptions = LayoutOptions.Start,
				//				VerticalOptions = LayoutOptions.End,
				                VerticalOptions = LayoutOptions.EndAndExpand,
				SeparatorColor = Color.Transparent,
				IsEnabled = false,
//				HeightRequest = 90,
			};
			_listUtenze.SetBinding(ListView.ItemsSourceProperty, "ListUtenza");
//			_listUtenze.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleListUtenze()));
			_listUtenze.SetBinding(ListView.HeightRequestProperty, new Binding("ListUtenza", converter: new ListUtenzeHeight()));

			layoutListaUtenza = new StackLayout() 
			{
				//				Padding = new Thickness(10, 10, 10, 10),
//				BackgroundColor = ColorHelper.White, 
//				Orientation = StackOrientation.Horizontal, 
				VerticalOptions = LayoutOptions.EndAndExpand,
				//				Spacing = 5, 
			};
			layoutListaUtenza.SetBinding(StackLayout.HeightRequestProperty, new Binding("ListUtenza", converter: new ListUtenzeHeight()));
			layoutListaUtenza.SetBinding(StackLayout.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleListUtenze()));
			layoutListaUtenza.Children.Add(_listUtenze);

            

            _lblTitleUtenza = new Label()
            {
                Text = "USO UTENZA",
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            _lblTitleUtenza.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleUsoUtenza()));

            _lblUtenza = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                VerticalOptions = LayoutOptions.EndAndExpand,
            };
            _lblUtenza.SetBinding(Label.TextProperty, "UsoUtenza");
            _lblTitleUtenza.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleUsoUtenza()));


            _activityIndicator = new ActivityIndicator()
            {
                IsRunning = false,
                IsVisible = false,
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

//            var layoutUtenza = new StackLayout() 
//            {
//				Padding = new Thickness(10, 10, 10, 10),
//                BackgroundColor = ColorHelper.White, 
//                Orientation = StackOrientation.Horizontal, 
//                VerticalOptions = LayoutOptions.EndAndExpand,
//                Spacing = 5, 
//                Children = { _lblTitleUtenza, _lblUtenza } 
//            };
//            layoutUtenza.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleUsoUtenza()));

            
			var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Spacing = 5,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = 
                {
//                    new StackLayout() {Padding = new Thickness(15, 10, 15, 10), BackgroundColor = ColorHelper.White, Orientation = StackOrientation.Horizontal, Spacing = 5, Children = {_lblDay, _lblDate}},
                    _lblInfo,
                    _listView,
					layoutListaUtenza,
					_activityIndicator,
//                    layoutUtenza,
//					_listUtenze
                }
            };

			MessagingCenter.Subscribe<TabbedHomeView, bool>(this, "sync", (sender, arg2) => {
				if (arg2)
				{
					_activityIndicator.IsRunning = true;
					_activityIndicator.IsVisible = true;
//					await Task.Run(() =>
//					{
//						Device.BeginInvokeOnMainThread(() => fab.Hide());
//					});
				}
				else
				{
					_activityIndicator.IsRunning = false;
					_activityIndicator.IsVisible = false;
//					await Task.Run(() =>
//					{
//						Device.BeginInvokeOnMainThread(() => fab.Show());
//					});
				}
			});

			var absolute = new AbsoluteLayout() { 
				VerticalOptions = LayoutOptions.FillAndExpand, 
				HorizontalOptions = LayoutOptions.FillAndExpand 
			};

			// Position the pageLayout to fill the entire screen.
			// Manage positioning of child elements on the page by editing the pageLayout.
			AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0f, 0f, 1f, 1f));
			absolute.Children.Add(layout);

			// Overlay the FAB in the bottom-right corner
			AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			absolute.Children.Add(fab);
	
			return absolute;

//            return layout;
        }
        #endregion

        #region Event Handlers
//		async void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
//		{
//			if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
//				return;
//			var orario = (Orari)_listView.SelectedItem;
//
//			string action;
//			//if(_db.CheckAppartieneMieiCorsi(orario))
//			action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Rimuovi dai preferiti");
//
//			//else
//			//    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Dettagli", "Aggiungi ai preferiti");
//
//			switch (action)
//			{
//			case "Rimuovi dai preferiti":
//				var conferma = await DisplayAlert("RIMUOVI", string.Format("Sei sicuro di volere rimuovere {0} dai corsi preferiti?", orario.Insegnamento), "Conferma", "Annulla");
//				if (conferma)
//				{
//					var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
//					_db.DeleteMieiCorsi(corso);
//					MessagingCenter.Send<TabbedDayView>(this, "delete_corso");
//				}
//				else
//					return;
//				break;
//				//case "Aggiungi ai preferiti":
//				//    _db.Insert(new MieiCorsi() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
//				//    break;
//			default:
//				//case "Dettagli":
//				//    var nav = new DettagliCorsoView();
//				//    //nav.BindingContext = 
//				//    await this.Navigation.PushAsync(nav);
//				break;
//			}
//
//			((ListView)sender).SelectedItem = null;
//		}

//        private void sync(TabbedHomeView arg1, bool arg2)
//        {
//            if (arg2)
//            {
//                _activityIndicator.IsRunning = true;
//                _activityIndicator.IsVisible = true;
//            }
//            else
//            {
//                _activityIndicator.IsRunning = false;
//                _activityIndicator.IsVisible = false;
//            }
//        }

        #endregion

		#region Override
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			_viewModel = (Giorno)BindingContext;

		}
		#endregion
    }

    #region Converter
    public class IsVisibleListUtenze : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Utenza>)
            {
                var x = (IEnumerable<Utenza>)value;
                if (x.Count() > 0)
                    return true;
                else return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

	public class ListUtenzeHeight : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is IEnumerable<Utenza>)
			{
				var x = (IEnumerable<Utenza>)value;
				return 45*x.Count();
			}

			return 0;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

    public class IsVisibleUsoUtenza : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Utenza>)
            {
                var x = (IEnumerable<Utenza>)value;
                if (x.Count() == 1)
                    return true;
                else return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsVisibleCountConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Orari>)
            {
                var x = (IEnumerable<Orari>)value;
                switch (x.Count())
                {
                    case 0:
                        return true;
                    default:
                        return false;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
    #endregion 




}
