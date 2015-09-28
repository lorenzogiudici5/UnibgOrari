using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.Helpers;
using Toasts.Forms.Plugin.Abstractions;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioComplCellGroup : ViewCell
    {
        #region Constructor
        public OrarioComplCellGroup()
        {
			_db = new DbSQLite();
			setUpContextAction ();
            View = getView();
        }
        #endregion

        #region Private Fields
        private StackLayout _layout;
        private Label _lblInsegnamento;
        private Label _lblDocente;
        private Label _lblAulaOra;
		private DbSQLite _db;
		private Xamarin.Forms.MenuItem addAction;
        #endregion

        #region Private Methods
		private View getView()
		{
			_lblInsegnamento = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				TextColor = ColorHelper.Blue,
			};

			_lblDocente = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				FontAttributes = Xamarin.Forms.FontAttributes.Bold,
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};

			_lblAulaOra = new Label()
			{
				TextColor = ColorHelper.Gray,
				FontSize = Device.GetNamedSize(NamedSize.Small, this)
			};

			_lblInsegnamento.SetBinding(Label.TextProperty, "Insegnamento");
			_lblDocente.SetBinding(Label.TextProperty, "Docente");
			//lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");

			_lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

			MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "select_fav", (sender, arg) =>
				{
					if(arg.Insegnamento == _lblInsegnamento.Text)
						_layout.BackgroundColor = ColorHelper.LightBlue;
				});
			MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "deselect_fav", (sender, arg) =>
				{
					if (arg.Insegnamento == _lblInsegnamento.Text)
						_layout.BackgroundColor = ColorHelper.White;
				});

			_layout = new StackLayout()
			{
				BackgroundColor = ColorHelper.White,
				Padding = new Thickness(10, 10, 10, 10),
				Children = 
				{ 
					_lblInsegnamento,
					new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {_lblAulaOra, _lblDocente}},
				}
				};
			return _layout;
		}

		private void setUpContextAction()
		{
			//ADD
			addAction = new Xamarin.Forms.MenuItem { Text="Aggiungi ai preferiti"};
			addAction.SetBinding(Xamarin.Forms.MenuItem.CommandParameterProperty, new Binding ("."));
			addAction.Clicked += AddAction_Clicked;

			ContextActions.Add (addAction);
		}
        #endregion

		#region Event Handlers
		async void AddAction_Clicked (object sender, EventArgs e)
		{
			var mi = ((Xamarin.Forms.MenuItem)sender);
			var orario = mi.CommandParameter as CorsoGiornaliero;
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
}
