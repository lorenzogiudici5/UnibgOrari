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
using Xamarin;
using OrariUnibg.Services;
using Refractored.XamForms.PullToRefresh;
using System.Diagnostics;
using Plugin.Connectivity;
using Plugin.Toasts;
using OrariUnibg.ViewModels;

namespace OrariUnibg.Views
{
    public class TabbedDayView : ContentPage
    {
        #region Constructor
        public TabbedDayView()
        {
            _db = new DbSQLite();
            this.SetBinding(ContentPage.TitleProperty, "Day");

            Logcat.Write(string.Format("{0}: {1}", "TABBEDDAYVIEW", "before content"));

            //Content = getView();
            Content = getPullToRefreshView();
        }
        #endregion

        //#region Properties
        //public DayViewModel ViewModel { get { return _viewModel; } set { if (_viewModel != value) _viewModel = value; } }
        //#endregion

        #region Private Fields
        private ListView _listView;
//        private Label _lblDay;
//        private Label _lblDate;
        private Label _lblInfo;
        private Label _lblTitleUtenza;
        private Label _lblUtenza;
        //private ActivityIndicator _activityIndicator;
        private DbSQLite _db;
        private ListView _listUtenze;
		private StackLayout layoutListaUtenza;
		private DayViewModel _viewModel;
        PullToRefreshLayout _refreshView;
        #endregion

        #region Private Methods
        private View getPullToRefreshView()
        {
            var scrollView = new ScrollView
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = getView() /* Anything you want in your ScrollView */
            };

            _refreshView = new PullToRefreshLayout
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Content = scrollView,
                RefreshColor = Color.FromHex("#3498db"),
            };

            var refreshCommand = new Command( () => //(async () =>
            {
                sync();
                //_refreshView.IsRefreshing = true;
                //await _db.SynchronizeAzureDb();

                //await updateDbOrariUtenza();

                //MessagingCenter.Send<TabbedDayView, int>(this, "pullToRefresh", 0);
                //Debug.WriteLine("Command executed");
                //updateLabelInfo(); //aggiorna la label delle information (da aggiornare oppure rilassati)
                //_refreshView.IsRefreshing = false;
            });

            _refreshView.RefreshCommand = refreshCommand;

            //Set Bindings
            //_refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.IsRefreshingProperty, vm => vm.IsBusy, BindingMode.OneWay);
            //_refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.RefreshCommandProperty, command);
            //_refreshView.SetBinding<TestViewModel>(PullToRefreshLayout.RefreshCommandProperty, vm => vm.RefreshCommand);


            return _refreshView;
        }
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
					share();
				}
			};
			fab.SetBinding(Label.IsVisibleProperty, new Binding("ListaLezioni", converter: new IsINVisibleCountConverter()));
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
            updateLabelInfo();

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


            //_activityIndicator = new ActivityIndicator()
            //{
            //    IsRunning = true,
            //    IsVisible = false,
            //    VerticalOptions = LayoutOptions.EndAndExpand,
            //};

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
					//_activityIndicator,
//                    layoutUtenza,
//					_listUtenze
                }
            };

			//MessagingCenter.Subscribe<TabbedHomeView, bool>(this, "sync", (sender, arg2) => {
			//	if (arg2)
			//		_activityIndicator.IsVisible = true;
			//	else
			//		_activityIndicator.IsVisible = false;
			//});

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

        private async Task updateDbOrariUtenza()
        {
            var _listOrariGiorno = _db.GetAllOrari(); //Elimina gli orari già passati

            //***TO CHECK! 
            foreach (var l in _listOrariGiorno)
            {
                if (l.Date < DateTime.Today.Date) //se l'orario è di ieri lo cancello
                    _db.DeleteSingleOrari(l.IdOrario);
            };

            if (!CrossConnectivity.Current.IsConnected)
            { //non connesso a internet
                var toast = DependencyService.Get<IToastNotificator>();
                await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                return;
            }

            foreach (var day in _viewModel.ListGiorni)
            {
                //Corsi generale, utenza + corsi
                var db = Settings.FacoltaDB;
                string s = await Web.GetOrarioGiornaliero(Settings.FacoltaDB, Settings.FacoltaId, 0, day.DateString);
                List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, day.Data);

                if (listaCorsi.Count() != 0)
                    updateSingleCorso(_db, listaCorsi);
            }

            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();
            _db.CheckUtenzeDoppioni();

            //Settings.LastUpdate = DateTime.Now.ToString ("R");
            //Settings.ToUpdate = false;
            Settings.LastUpdate = DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss");
        }


        private void updateLabelInfo()
        {
            if (Settings.ToUpdate)
                _lblInfo.Text = "Trascina verso il basso per aggiornare";
            else
                _lblInfo.Text = "Rilassati! Non hai lezioni!";

            _lblInfo.TextColor = ColorHelper.Green500;
        }

        private void updateLabelInfo(string text, Color color)
        {
            _lblInfo.Text = text;
            _lblInfo.TextColor = color;
        }

        private async void sync()
        {
            _refreshView.IsRefreshing = true;

            var toast = DependencyService.Get<IToastNotificator>();
            if (!CrossConnectivity.Current.IsConnected)
            { //non connesso a internet
                
                await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                return;
            }

            bool success = false;
            for (int i = 0; i < 3; i++)
            {
                success = await _db.SynchronizeAzureDb();

                if (!success)
                {
                    //await toast.Notify(ToastNotificationType.Error, "Errore", "Sincronizzazione fallita!", TimeSpan.FromSeconds(3));
                    if(i==2)
                        await toast.Notify(ToastNotificationType.Error, "Errore", "Sincronizzazione fallita!", TimeSpan.FromSeconds(3));

                    updateLabelInfo(string.Format("Errore sincronizzazione - Tentativo {0}", i+1),Color.Red);
                    continue; //riprovo
                }
                    
                await updateDbOrariUtenza();
                break;
            }

            MessagingCenter.Send<TabbedDayView, int>(this, "pullToRefresh", 0);
            Debug.WriteLine("Command executed");
            updateLabelInfo(); //aggiorna la label delle information (da aggiornare oppure rilassati)
            _refreshView.IsRefreshing = false;
        }
        #endregion

        #region Public Methods
        /**
		* Aggiorna il singolo corso, verificando se appartiene ai corsi e in caso, aggiorna, aggiunge o notifica
		* */
        public static void updateSingleCorso(DbSQLite _db, List<CorsoGiornaliero> listaCorsi)
        {
            foreach (var c in listaCorsi)
            {
                var corso = c;
                Logcat.Write("ORARI_UNIBG: prima di Check");

                if (_db.CheckAppartieneMieiCorsi(c))
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
                        Logcat.Write("Orario già PRESENTE nel DB: " + orario.Insegnamento);
                        var o = _db.GetAllOrari().FirstOrDefault(y => y.Insegnamento == orario.Insegnamento && y.Date.Date == orario.Date.Date && orario.AulaOra == y.AulaOra);

                        if ((string.Compare(o.Note, corso.Note) != 0) || !o.Notify)
                        {
                            o.Note = corso.Note;
                            o.AulaOra = corso.AulaOra;
                            if (o.Note != null && o.Note != "" && !o.Notify)
                            {
                                DependencyService.Get<INotification>().SendNotification(corso);
                                o.Notify = true;
                            }
                            _db.Update(o);
                        }
                    }
                    else // l'orario non è presente nel mio db
                    {
                        orario.Notify = false;
                        Logcat.Write("Orario NUOVO" + orario.Insegnamento);
                        if (orario.Note != null && orario.Note != "" && !orario.Notify)
                        {
                            DependencyService.Get<INotification>().SendNotification(corso);
                            //SendNotification(corso);
                            orario.Notify = true;
                        }

                        _db.Insert(orario);
                    }
                }

                else if (corso.Insegnamento.Contains("UTENZA")) //verifico se è un utenza
                {
                    Utenze ut = new Utenze() { Data = corso.Date, AulaOra = corso.AulaOra };
                    if (!_db.AppartieneUtenze(ut))
                        _db.Insert(ut);
                }
            }
        }
        #endregion

        #region Event Handlers
        private void share()
		{
			string text = _viewModel.ToString ();
			string s = "Condividi Testo";
			//possibilità di condividere solo il testo

//			var s = await DisplayActionSheet ("Condividi", "Annulla", null, "Visualizza PDF", "Condividi PDF", "Condividi Testo");
//			if (s.Contains("PDF")) {
//				PdfFile pdf = new PdfFile () { Title = "Orario giornaliero", Text = text };
//				pdf.CreateGiornaliero ();
//
//				await pdf.Save ();
//				if(s.Contains("Condividi")) //Condividi PDF
//					DependencyService.Get<IFile> ().Share (pdf._filename);
//				else
//					await pdf.Display (); //visualizza PDF
//			} 
//			else{
//				text+= Settings.Firma;
//				DependencyService.Get<IMethods> ().Share (text); //condividi testo
//			}

			text+= Settings.Firma;
			DependencyService.Get<IMethods> ().Share (text); //condividi testo

			//Insights.Track("Share", new Dictionary <string,string>{
			//	{"Orario", "Preferiti_"+s},
			//});

		}
        #endregion

		#region Override
		protected override void OnBindingContextChanged()
		{
			base.OnBindingContextChanged();
			_viewModel = (DayViewModel)BindingContext;

		}

        protected override void OnAppearing()
        {
            //if (Settings.ToUpdate)
            //    sync();
            updateLabelInfo();
            base.OnAppearing();
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

    public class IsVisibleCountConverter : IValueConverter //PER LA LABEL nessuna lezione
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (Settings.ToUpdate)
                return true;

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

	public class IsINVisibleCountConverter : IValueConverter //per il FAB BUTTON
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is IEnumerable<Orari>)
			{
				var x = (IEnumerable<Orari>)value;
				switch (x.Count())
				{
				case 0:
					return false;
				default:
					return true;
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
