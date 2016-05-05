using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using Plugin.Toasts;
using OrariUnibg.ViewModels;
using OrariUnibg.Services.Azure;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioComplCell : ViewCell
    {
		#region Constructor
		public OrarioComplCell()
		{
			_db = new DbSQLite();
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
        private DbSQLite _db;
		private Xamarin.Forms.MenuItem addAction;
		#endregion

		#region Private Methods
		private View getView()
		{
			var lblCorso = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Medium, this),
				TextColor = ColorHelper.Blue700,
			};

			var lblDocente = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				FontAttributes = Xamarin.Forms.FontAttributes.Bold,
				TextColor = Color.Black,
				HorizontalOptions = LayoutOptions.EndAndExpand,
			};

			var lblInizioFine = new Label()
			{
				FontSize = Device.GetNamedSize(NamedSize.Small, this),
				FontAttributes = Xamarin.Forms.FontAttributes.Italic,
				HorizontalOptions = LayoutOptions.Start,
				IsVisible = false,
			};

			var Lunedi = new Label() { Text = "LUNEDI:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray};
			var Martedi = new Label() { Text = "MARTEDI:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray };
			var Mercoledi = new Label() { Text = "MERCOLEDI:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray };
			var Giovedi = new Label() { Text = "GIOVEDI:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray };
			var Venerdi = new Label() { Text = "VENERDI:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray };
			var Sabato = new Label() { Text = "SABATO:", FontSize = Device.GetNamedSize(NamedSize.Small, this), TextColor = ColorHelper.Gray };



			var lblLunedi = new Label() { TextColor = ColorHelper.Gray };
			lblLunedi.FontSize = Device.GetNamedSize(NamedSize.Small, this);
			var lblMartedi = new Label() { TextColor = ColorHelper.Gray };
			lblMartedi.FontSize = Device.GetNamedSize(NamedSize.Small, this);
			var lblMercoledi = new Label() { TextColor = ColorHelper.Gray };
			lblMercoledi.FontSize = Device.GetNamedSize(NamedSize.Small, this);
			var lblGiovedi = new Label() { TextColor = ColorHelper.Gray };
			lblGiovedi.FontSize = Device.GetNamedSize(NamedSize.Small, this);
			var lblVenerdi = new Label() { TextColor = ColorHelper.Gray };
			lblVenerdi.FontSize = Device.GetNamedSize(NamedSize.Small, this);
			var lblSabato = new Label() { TextColor = ColorHelper.Gray };
			lblSabato.FontSize = Device.GetNamedSize(NamedSize.Small, this);

			lblCorso.SetBinding(Label.TextProperty, "Insegnamento");
			lblDocente.SetBinding(Label.TextProperty, "Docente");
			lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");

			lblLunedi.SetBinding(Label.TextProperty, "Lezioni[0].AulaOra");
			lblLunedi.SetBinding(Label.IsVisibleProperty, "Lezioni[0].isVisible");
			Lunedi.SetBinding(Label.IsVisibleProperty, "Lezioni[0].isVisible");

			lblMartedi.SetBinding(Label.TextProperty, "Lezioni[1].AulaOra");
			lblMartedi.SetBinding(Label.IsVisibleProperty, "Lezioni[1].isVisible");
			Martedi.SetBinding(Label.IsVisibleProperty, "Lezioni[1].isVisible");

			lblMercoledi.SetBinding(Label.TextProperty, "Lezioni[2].AulaOra");
			lblMercoledi.SetBinding(Label.IsVisibleProperty, "Lezioni[2].isVisible");
			Mercoledi.SetBinding(Label.IsVisibleProperty, "Lezioni[2].isVisible");

			lblGiovedi.SetBinding(Label.TextProperty, "Lezioni[3].AulaOra");
			lblGiovedi.SetBinding(Label.IsVisibleProperty, "Lezioni[3].isVisible");
			Giovedi.SetBinding(Label.IsVisibleProperty, "Lezioni[3].isVisible");

			lblVenerdi.SetBinding(Label.TextProperty, "Lezioni[4].AulaOra");
			lblVenerdi.SetBinding(Label.IsVisibleProperty, "Lezioni[4].isVisible");
			Venerdi.SetBinding(Label.IsVisibleProperty, "Lezioni[4].isVisible");

			lblSabato.SetBinding(Label.TextProperty, "Lezioni[5].AulaOra");
			lblSabato.SetBinding(Label.IsVisibleProperty, "Lezioni[5].isVisible");
			Sabato.SetBinding(Label.IsVisibleProperty, "Lezioni[5].isVisible");

			var label = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 0,
				Children = { Lunedi, Martedi, Mercoledi, Giovedi, Venerdi, Sabato }
			};
			var lblLezioni = new StackLayout()
			{
				Orientation = StackOrientation.Vertical,
				Spacing = 0,
				Children = { lblLunedi, lblMartedi, lblMercoledi, lblGiovedi, lblVenerdi, lblSabato}
			};

			var layout = new StackLayout()
			{
				BackgroundColor = ColorHelper.White,
				Padding = new Thickness(10, 10, 10, 10),
				Children = 
				{ 
					lblCorso,
					new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {label, lblLezioni}},
					new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {lblInizioFine, lblDocente}},
				}
				};
		
			var layoutExt = new StackLayout () {
				Padding = new Thickness (0, 3, 0, 3), 
				BackgroundColor = ColorHelper.Transparent,
			};
			layoutExt.Children.Add (layout);
			return layoutExt;
		}

		private void setUpContextAction()
		{
			if (!Settings.SuccessLogin) { //se non sono loggato, non posso aggiungere o rimuovere corsi dai preferiti
				return;
			}
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
			var orario = mi.CommandParameter as CorsoCompleto ;

			var toast = DependencyService.Get<IToastNotificator>();
			if (_db.CheckAppartieneMieiCorsi (orario)) {
				await toast.Notify (ToastNotificationType.Error, "Attenzione!", orario.Insegnamento + " è già stato aggiunto ai tuoi preferiti!", TimeSpan.FromSeconds (3));
			} else {
                await _service.Initialize();
                var preferito = new Preferiti() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento };
                var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };

                await _service.AddCorso(corso);
                corso = await _service.GetCorso(corso);

                preferito.IdCorso = corso.Id;
                await _service.AddPreferito(preferito);

                _db.Insert(preferito);
				await toast.Notify (ToastNotificationType.Success, "Complimenti", orario.Insegnamento + " aggiunto ai preferiti!", TimeSpan.FromSeconds (3));
			}
		}
		#endregion

    }
}
