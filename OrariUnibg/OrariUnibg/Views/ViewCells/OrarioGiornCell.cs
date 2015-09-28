using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using System.Diagnostics;
using Toasts.Forms.Plugin.Abstractions;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioGiornCell : ViewCell
    {
        #region Constructor
        public OrarioGiornCell()
        {
			_db = new DbSQLite();
			setUpContextAction ();
            View = getView();
        }
        #endregion
        #region Private Fields
        private Label _lblCorso;
        private Label _lblCodice;
        private Label _lblAula;
        private Label _lblOra;
        private Label _lblDocente;
        private Label _lblNote;
		private DbSQLite _db;
		private Orari orario;
//		private Xamarin.Forms.MenuItem removeAction;
		private Xamarin.Forms.MenuItem addAction;
        #endregion

        #region Private Methods
        private View getView()
		{
            _lblCorso = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Default, this),
                TextColor = ColorHelper.Blue,
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
				Padding = new Thickness (5, 0, 0, 0), 
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

			return layoutExt;
	

        }

		private void setUpContextAction()
		{
//			removeAction = new Xamarin.Forms.MenuItem { Text = "Rimuovi", Icon = "ic_bin.png" };
//			removeAction.SetBinding (Xamarin.Forms.MenuItem.CommandParameterProperty, new Binding ("."));
//			removeAction.Clicked += RemoveAction_Clicked;
			//ADD
			addAction = new Xamarin.Forms.MenuItem { Text="Aggiungi ai preferiti"};//, Icon = "ic_nostar.png"};
			addAction.SetBinding(Xamarin.Forms.MenuItem.CommandParameterProperty, new Binding ("."));
			addAction.Clicked += AddAction_Clicked;

			ContextActions.Add (addAction);
//			ContextActions.Add (removeAction);

//			if (_db.CheckAppartieneMieiCorsi (orario)) {
//				ContextActions.Add (removeAction);
//			} else {
//				ContextActions.Add (addAction);
//			}
		}
			
        #endregion

		#region Event Handlers
//		void RemoveAction_Clicked (object sender, EventArgs e)
//		{
//			var mi = ((Xamarin.Forms.MenuItem)sender);
//			var orario = mi.CommandParameter as Orari;
//
//			var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
//			_db.DeleteMieiCorsi(corso);
//
//			MessagingCenter.Send<OrarioGiornCell>(this, "delete_corso_context");
//			Debug.WriteLine(orario.Insegnamento);
//		}

		async void AddAction_Clicked (object sender, EventArgs e)
		{
			var mi = ((Xamarin.Forms.MenuItem)sender);
			var orario = mi.CommandParameter as CorsoGiornaliero;
			var x = _db.GetAllMieiCorsi ();

			var toast = DependencyService.Get<IToastNotificator>();
			if (_db.CheckAppartieneMieiCorsi (orario)) {
				await toast.Notify (ToastNotificationType.Error, "Attenzione!", orario.Insegnamento + " è già stato aggiunto ai tuoi preferiti!", TimeSpan.FromSeconds (3));
			} else {
				_db.Insert(new MieiCorsi() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
				await toast.Notify (ToastNotificationType.Success, "Complimenti", orario.Insegnamento + " aggiunto ai preferiti!", TimeSpan.FromSeconds (3));
			}
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
					return Color.FromHex("FF6666");
				case "Cambio aula":
					return Color.FromHex("FFFF66");
				case "Attività accademica":
					return Color.FromHex("B0B0FF");
				case "Attività integrativa":
					return Color.Pink;
				case "Esame":
					return Color.FromHex("A0FFA0");
				case "Alta formazione":
					return Color.FromHex("A0FFFF");
				case "Recupero lezione":
					return Color.FromHex("00DD00");
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
