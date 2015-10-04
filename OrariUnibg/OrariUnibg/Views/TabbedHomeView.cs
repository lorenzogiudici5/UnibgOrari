using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Services.Database;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using Connectivity.Plugin;
using Toasts.Forms.Plugin.Abstractions;
using OrariUnibg.Views.ViewCells;

namespace OrariUnibg.Views
{
    public class TabbedHomeView : TabbedPage
    {
        #region Constructor
        public TabbedHomeView()
        {
            _db = new DbSQLite();
			Logcat.Write("COUNT: " + _db.GetAllMieiCorsi().Count());
            this.Title = "Home";
            //BackgroundColor = ColorHelper.White;

			checkDays (); //controllo che giorni sono necessari nelle tab
			loadListCorsiGiorno(); //carico la lista dei giorni         

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new TabbedDayView();
            });

            tbiSync = new ToolbarItem("Sync", "ic_sync.png", sync, 0, 0);
            ToolbarItems.Add(tbiSync);


			MessagingCenter.Subscribe<TabbedDayView>(this, "delete_corso", (sender) => {
				loadListCorsiGiorno();
			});
			MessagingCenter.Subscribe<OrarioFavCell> (this, "delete_corso_fav", (sender) => {
				loadListCorsiGiorno();
			});

			MessagingCenter.Subscribe<OrarioFavCell> (this, "delete_corso_fav_impostazioni", (sender) => {
				loadListCorsiGiorno();
			});
//			MessagingCenter.Subscribe<OrarioGiornCell>(this, "delete_corso_context", deleteMioCorsoContext);
//			MessagingCenter.Subscribe<TabbedDayView>(this, "delete_corso", loadListCorsiGiorno());
        }
        #endregion

        #region Private Fields
        private DbSQLite _db;
        private List<Giorno> listGiorni;
        private Giorno _oggi;
        private Giorno _domani;
        private Giorno _dopodomani;
        private ToolbarItem tbiSync;
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
						Logcat.Write ("Orario già PRESENTE nel DB: " + orario.Insegnamento);
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
						Logcat.Write ("Orario NUOVO" +  orario.Insegnamento);
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
					Utenze ut = new Utenze() { Data = corso.Date, AulaOra = corso.AulaOra};
					if(!_db.AppartieneUtenze(ut))
						_db.Insert(ut); 
				}
			}

			//Settings.LastUpdate = DateTime.Now.ToString ("R");
			Settings.LastUpdate = DateTime.Now.ToString ("dd/MM/yyyy HH:mm:ss");
		}

		public void checkDays()
		{
			if (DateTime.Now.Hour > Settings.UpdateHour) // && DateTime.Now.Minute > Settings.UpdateMinute)
			{
				_oggi = new Giorno() { Data = DateTime.Today.AddDays(1) };
				_domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
			}
			else
			{
				_oggi = new Giorno() { Data = DateTime.Today };
				_domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
				_dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
			}
		}
		#endregion

        #region Private Methods
		private void updateListaGiorni()
		{
			listGiorni = new List<Giorno>()
			{
				_oggi, _domani, _dopodomani
			};
			this.ItemsSource = listGiorni;
		}

		private async void sync()
		{
			MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", true);
			ToolbarItems.Clear();
			ToolbarItems.Remove(tbiSync);

			await updateDbOrariUtenza ();

			loadListCorsiGiorno();

			ToolbarItems.Add(tbiSync);
			MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", false);
		}

        private async Task updateDbOrariUtenza()
        {
            var _listOrariGiorno = _db.GetAllOrari(); //Elimina gli orari già passati

            foreach (var l in _listOrariGiorno)
            {
                if (l.Date < _oggi.Data)
                    _db.DeleteSingleOrari(l.Id);
            };

			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
				var toast = DependencyService.Get<IToastNotificator>();
				await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				return;
			}

			
			foreach (var day in listGiorni)
            {				
                //Corsi generale, utenza + corsi
				string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.FacoltaId, 0, day.DateString);
				List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, day.Data);

				if (listaCorsi.Count () != 0)
					updateSingleCorso (_db, listaCorsi);
            }

            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();
        }

        private void loadListCorsiGiorno()
        {
			var utenze = _db.GetAllUtenze ();
			_oggi.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data.Date, dateX.Date.Date) == 0);
//			_oggi.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _oggi.Data.Date);
			_oggi.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data.Date, dateX.Data.Date) == 0);
			var count = _oggi.ListUtenza.Count ();

			_domani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data.Date, dateX.Date.Date) == 0);
			_domani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data.Date, dateX.Data.Date) == 0);

			_dopodomani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dopodomani.Data.Date, dateX.Date.Date) == 0);
			_dopodomani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data.Date, dateX.Data.Date) == 0);

			updateListaGiorni();

			Settings.MieiCorsiCount = _db.GetAllMieiCorsi ().Count();
        }
        #endregion

        #region Event Handlers
		protected override bool OnBackButtonPressed ()
		{
			DependencyService.Get<IMethods>().Close_App(); //altrmenti nulla
			return true;
//			return true; //ALERT CHIUDERE APP??
		}
		protected async override void OnAppearing()
		{
			var utenze = _db.GetAllUtenze ();
			_db.CheckUtenzeDoppioni (); //verifica che non ci sono doppioni nelle utenze, non so perchè ma capita che me ne crea
			MessagingCenter.Send<TabbedHomeView, bool> (this, "sync", true);
			ToolbarItems.Remove(tbiSync);
			checkDays ();
			updateListaGiorni ();

//			var count = _db.GetAllMieiCorsi ().Count ();
			if (_db.GetAllMieiCorsi ().Count () > Settings.MieiCorsiCount) { // || listGiorni[0].Data != DateTime.Today) //ne è stato aggiunto uno nuovo, è cambiato giorno ATTENZIONE: domenica??
				await updateDbOrariUtenza ();
//				Settings.MieiCorsiCount = _db.GetAllMieiCorsi ().Count ();
			}
				
			loadListCorsiGiorno();
			MessagingCenter.Send<TabbedHomeView, bool> (this, "sync", false);
			ToolbarItems.Add(tbiSync);

			base.OnAppearing();

		}
        
        #endregion
    }
}
