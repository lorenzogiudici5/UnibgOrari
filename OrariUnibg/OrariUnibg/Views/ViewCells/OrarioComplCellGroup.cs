using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.Helpers;
using OrariUnibg.ViewModels;
using Plugin.Toasts;
using OrariUnibg.Services.Azure;
using Plugin.Connectivity;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioComplCellGroup : ViewCell
    {
        #region Constructor
        public OrarioComplCellGroup()
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
				TextColor = ColorHelper.Blue700,
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

			MessagingCenter.Subscribe<ListaCorsi, Preferiti>(this, "select_fav", (sender, arg) =>
				{
					if(arg.Insegnamento == _lblInsegnamento.Text)
						_layout.BackgroundColor = ColorHelper.LightBlue500;
				});
			MessagingCenter.Subscribe<ListaCorsi, Preferiti>(this, "deselect_fav", (sender, arg) =>
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
			var orario = mi.CommandParameter as CorsoCompletoGroupViewModel;
			var toast = DependencyService.Get<IToastNotificator>();
			//if (_db.CheckAppartieneMieiCorsi (orario)) {
			//	await toast.Notify (ToastNotificationType.Error, "Attenzione!", orario.Insegnamento + " è già stato aggiunto ai tuoi preferiti!", TimeSpan.FromSeconds (3));
			//} else {
			//	_db.Insert(new Preferiti() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
			//	await toast.Notify (ToastNotificationType.Success, "Complimenti", orario.Insegnamento + " aggiunto ai preferiti!", TimeSpan.FromSeconds (3));
			//}

            if (_db.CheckAppartieneMieiCorsi(orario))
            {
                await toast.Notify(ToastNotificationType.Error, "Attenzione!", orario.Insegnamento + " è già stato aggiunto ai tuoi preferiti!", TimeSpan.FromSeconds(3));
            }
            else
            {
                //**NON C'E CONNESSIONE INTERNET**
                if (!CrossConnectivity.Current.IsConnected)
                {   //non connesso a internet
                    await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                    return;
                }
                //await _service.Initialize();
                var preferito = new Preferiti() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento };
                var corso = new Corso() { Insegnamento = preferito.Insegnamento, Codice = preferito.Codice, Docente = preferito.Docente, };

                await _service.AddCorso(corso);
                corso = await _service.GetCorso(corso);

                preferito.IdCorso = corso.Id;
                await _service.AddPreferito(preferito);

                _db.Insert(preferito);
                await toast.Notify(ToastNotificationType.Success, "Complimenti", orario.Insegnamento + " aggiunto ai preferiti!", TimeSpan.FromSeconds(3));
            }

        }
		#endregion

    }
}
