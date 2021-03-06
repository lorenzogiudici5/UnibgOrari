﻿using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using System.Diagnostics;
using OrariUnibg.Helpers;

using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Plugin.Toasts;
using OrariUnibg.Services.Azure;
using Plugin.Connectivity;

namespace OrariUnibg
{
	class OrarioFavCell : ViewCell
	{
		#region Constructor
		public OrarioFavCell()
		{
			_db = new DbSQLite();
            _service = new AzureDataService();
            setUpContextAction ();
			View = getView();
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
        private Label _lblCorso;
		private Label _lblCodice;
		private Label _lblAula;
		private Label _lblOra;
		private Label _lblDocente;
		private Label _lblNote;
		private DbSQLite _db;
//		private Orari orario;
		private Xamarin.Forms.MenuItem removeAction;
		#endregion

		#region Private Methods
		private View getView()
		{
			_lblCorso = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Default, this),
				TextColor = ColorHelper.Blue700,
				//HorizontalOptions = LayoutOptions.FillAndExpand
			};
			_lblCorso.SetBinding(Label.TextProperty, "Insegnamento");

			_lblCodice = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Micro, this),
				TextColor = ColorHelper.Gray,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};
			_lblCodice.SetBinding(Label.TextProperty, "Codice");

			_lblAula = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				TextColor = ColorHelper.Gray,
				//HorizontalOptions = LayoutOptions.FillAndExpand
			};
			_lblAula.SetBinding(Label.TextProperty, "Aula");

			_lblOra = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				TextColor = ColorHelper.Gray,
				HorizontalOptions = LayoutOptions.FillAndExpand,
			};
			_lblOra.SetBinding(Label.TextProperty, "Ora");

			_lblDocente = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				FontAttributes = Xamarin.Forms.FontAttributes.Bold,
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};
			_lblDocente.SetBinding(Label.TextProperty, "Docente");

			_lblNote = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.StartAndExpand,
			};
			_lblNote.SetBinding(Label.TextProperty, "Note");
			_lblNote.SetBinding(Label.IsVisibleProperty, new Binding("Note", converter: new NoteIsVisibleConverter()));
			_lblNote.SetBinding(Label.TextColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

			//lblOra.SetBinding(Label.TextColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));
			//lblAula.SetBinding(Label.TextColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

			var grid = new Grid()
			{
				BackgroundColor = ColorHelper.White,
				Padding = new Thickness(10, 10, 10, 10),
				HorizontalOptions = LayoutOptions.FillAndExpand,
				VerticalOptions = LayoutOptions.FillAndExpand,
				RowSpacing = 0,
				ColumnSpacing = 5,

				RowDefinitions = 
				{
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
					new RowDefinition { Height = GridLength.Auto },
				},
				ColumnDefinitions = 
				{ 
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
				};
			//grid.SetBinding(Grid.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

			grid.Children.Add(_lblCorso, 0, 2, 0, 1);
			//grid.Children.Add(_lblCodice, 1, 2, 0, 1);
			grid.Children.Add(_lblAula, 0, 1, 1, 2);
			grid.Children.Add(_lblCodice, 1, 2, 1, 2);
			grid.Children.Add(_lblOra, 0, 1, 2, 3);
			grid.Children.Add(_lblDocente, 1, 2, 2, 3);
			grid.Children.Add(_lblNote, 0, 2, 3, 4);

			var layoutInt = new StackLayout () {
				Padding = new Thickness (6, 0, 0, 0), 
				HeightRequest = grid.Height,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};

			layoutInt.SetBinding(StackLayout.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));
			layoutInt.Children.Add (grid);

			var layoutExt = new StackLayout () {
				Padding = new Thickness (0, 3, 0, 3), 
				BackgroundColor = ColorHelper.Transparent,
			};
			layoutExt.Children.Add (layoutInt);

			//CONTEXT ACTION
			//			setUpContextAction ();

			return layoutExt;

			//var layout = new StackLayout()
			//{
			//    Padding = new Thickness(10, 10, 10, 10),
			//    Spacing = 0,
			//    VerticalOptions = LayoutOptions.FillAndExpand,
			//    HorizontalOptions = LayoutOptions.FillAndExpand,
			//    Orientation = StackOrientation.Vertical,
			//    Children = { 
			//        lblCorso, 
			//        lblAula, 
			//        new StackLayout(){HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, Children ={lblOra, lblDocente} },
			//        lblNote
			//    }
			//};

			//layout.SetBinding(StackLayout.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

			//MessagingCenter.Subscribe<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", (sender, arg) =>
			//{
			//    //if (arg.Insegnamento == _lblCorso.Text && arg.Ora == _lblOra.Text & arg.Aula == _lblAula.Text)
			//    //{
			//    //    ////DOUBLE TAPPED?!
			//    //    ////if (_lblNote.Text != String.Empty)
			//    //    ////{
			//    //    ////    if (_lblNote.IsVisible)
			//    //    ////        _lblNote.IsVisible = false;
			//    //    ////    else
			//    //    ////        _lblNote.IsVisible = true;
			//    //    ////}

			//    //    ////if (_lblCodice.IsVisible)
			//    //    ////    _lblCodice.IsVisible = false;
			//    //    ////else
			//    //    ////    _lblCodice.IsVisible = true;
			//    //}
			//    //else
			//    //    return;
			//});

			//MessagingCenter.Subscribe<TabbedDayView, Orari>(this, "orari_clicked", (sender, arg) =>
			//{
			//    if (arg.Insegnamento == _lblCorso.Text && arg.Ora == _lblOra.Text & arg.Aula == _lblAula.Text)
			//    {
			//        if (_lblNote.Text != String.Empty)
			//        {
			//            if (_lblNote.IsVisible)
			//                _lblNote.IsVisible = false;
			//            else
			//                _lblNote.IsVisible = true;
			//        }

			//        if (_lblCodice.IsVisible)
			//            _lblCodice.IsVisible = false;
			//        else
			//            _lblCodice.IsVisible = true;

			//    }
			//    else
			//        return;
			//});

		}

		private void setUpContextAction()
		{
			removeAction = new Xamarin.Forms.MenuItem { Text = "Rimuovi dai preferiti" }; //, Icon = "ic_bin.png" };
			removeAction.SetBinding (Xamarin.Forms.MenuItem.CommandParameterProperty, new Binding ("."));
			removeAction.Clicked += RemoveAction_Clicked;


			ContextActions.Add (removeAction);
		}

		#endregion

		#region Event Handlers
		async void RemoveAction_Clicked (object sender, EventArgs e)
		{
            var mi = ((Xamarin.Forms.MenuItem)sender);
            //var preferito = mi.CommandParameter as Preferiti;
            ////var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
            //var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };

            //corso = await _service.GetCorso(preferito);
            //preferito.IdCorso = corso.Id;
            //await _service.DeletePreferito(preferito);
            
            var orario = mi.CommandParameter as Orari;
            var toast = DependencyService.Get<IToastNotificator>();

            //**NON C'E CONNESSIONE INTERNET**
            if (!CrossConnectivity.Current.IsConnected)
            {   //non connesso a internet
                await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                return;
            }

            var preferito = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
            var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };
            corso = await _service.GetCorso(preferito);
            preferito.IdCorso = corso.Id;
            await _service.DeletePreferito(preferito);

            _db.DeleteMieiCorsi(preferito);

            MessagingCenter.Send<OrarioFavCell>(this, "delete_corso_fav");

            await toast.Notify(ToastNotificationType.Error, "Complimenti", orario.Insegnamento + " rimosso dai preferiti!", TimeSpan.FromSeconds(3));

            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();
        }
		#endregion

	}

	#region Converter
	public class NoteBackgroundConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is string)
			{
				switch ((string)value)
				{
				case "Sospensione lezione":
					return ColorHelper.Red500;
				case "Cambio aula":
					return ColorHelper.Yellow500;
				case "Attività accademica":
					return ColorHelper.DeepPurple500;
				case "Attività integrativa":
					return ColorHelper.Pink500;
				case "Esame":
					return ColorHelper.LightGreen500;
				case "Alta formazione":
					return ColorHelper.CyanA100;
				case "Recupero lezione":
					return ColorHelper.Green500;
				default:
					return Color.White; // per sfondo layout
				}
			}

			return string.Empty;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}

	public class NoteIsVisibleConverter : IValueConverter
	{

		public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			if (value is string)
			{
				if ( (string)value != string.Empty)
					return true;
				else 
					return false;
			}

			return false;
		}

		public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
		{
			throw new NotImplementedException();
		}
	}
	#endregion
}

