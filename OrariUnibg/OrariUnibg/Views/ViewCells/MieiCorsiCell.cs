using System;
using System.Collections.Generic;
using System.Linq;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using Plugin.Toasts;
using OrariUnibg.Services.Azure;
using OrariUnibg.Models;

namespace OrariUnibg
{
	public class MieiCorsiCell : ViewCell
	{
		#region Constructor
		public MieiCorsiCell ()
		{
			_db = new DbSQLite ();
            _service = new AzureDataService();
			setUpContextAction ();
			View = getView ();
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
		private Label _lblDocente;
		private Xamarin.Forms.MenuItem removeAction;
		private DbSQLite _db;
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

			_lblDocente = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				FontAttributes = Xamarin.Forms.FontAttributes.Bold,
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};
			_lblDocente.SetBinding(Label.TextProperty, "Docente");

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
				},
				ColumnDefinitions = 
				{ 
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
					new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
				}
				};
			//grid.SetBinding(Grid.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

			grid.Children.Add(_lblCorso, 0, 2, 0, 1);
			grid.Children.Add(_lblCodice, 1, 2, 1, 2);
			grid.Children.Add(_lblDocente, 1, 2, 2, 3);

			var layoutInt = new StackLayout () {
				Padding = new Thickness (5, 0, 0, 0),
				BackgroundColor = ColorHelper.White,
				HeightRequest = grid.Height,
				VerticalOptions = LayoutOptions.FillAndExpand,
			};
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
			var preferito = mi.CommandParameter as Preferiti;
            //var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
            var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };

            corso = await _service.GetCorso(preferito);
            preferito.IdCorso = corso.Id;
            await _service.DeletePreferito(preferito);

			_db.DeleteMieiCorsi(preferito);

			MessagingCenter.Send<MieiCorsiCell>(this, "delete_corso_fav_impostazioni");

			var toast = DependencyService.Get<IToastNotificator>();
			await toast.Notify (ToastNotificationType.Error, "Complimenti", corso.Insegnamento + " rimosso dai preferiti!", TimeSpan.FromSeconds (3));
		
			Settings.MieiCorsiCount = _db.GetAllMieiCorsi ().Count ();
		}
		#endregion
	}
}

