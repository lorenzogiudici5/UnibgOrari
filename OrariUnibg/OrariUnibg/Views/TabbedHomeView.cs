﻿using OrariUnibg.Models;
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

namespace OrariUnibg.Views
{
    public class TabbedHomeView : TabbedPage
    {
        #region Constructor
        public TabbedHomeView()
        {
            _db = new DbSQLite();
			System.Diagnostics.Debug.WriteLine("COUNT: " + _db.GetAllMieiCorsi().Count());
            this.Title = "Home";
            //BackgroundColor = ColorHelper.White;

			checkDays (); //controllo che giorni sono necessari nelle tab
			loadListCorsiGiorno(); //carico la lista dei giorni

//            _oggi.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data, dateX.Date) == 0);
//            _oggi.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _oggi.Data);
//            
//            _domani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data, dateX.Date) == 0);
//            _domani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _domani.Data);
//            
//            _dopodomani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dopodomani.Data, dateX.Date) == 0);
//            _dopodomani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _dopodomani.Data);
//
//
//			listGiorni = new List<Giorno>()
//            {
//                _oggi, _domani, _dopodomani
//            };
//			this.ItemsSource = listGiorni;            

            this.ItemTemplate = new DataTemplate(() =>
            {
                return new TabbedDayView();
            });

            tbiSync = new ToolbarItem("Sync", "ic_sync.png", sync, 0, 0);
            ToolbarItems.Add(tbiSync);


            MessagingCenter.Subscribe<TabbedDayView>(this, "delete_corso", deleteMioCorso);
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
				System.Diagnostics.Debug.WriteLine ("ORARI_UNIBG: prima di Check");
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
						var o = _db.GetAllOrari().FirstOrDefault(y => y.Insegnamento == orario.Insegnamento && y.Date.Date == orario.Date.Date);

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
					Utenza ut = new Utenza() { Data = corso.Date, AulaOra = corso.AulaOra};
					if(!_db.AppartieneUtenze(ut))
						_db.Insert(ut); 
				}
			}
		}

		public void checkDays()
		{
			if (DateTime.Now.Hour > Settings.UpdateHour && DateTime.Now.Minute > 35)
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

			await updateDbOrariUtenza();
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

			foreach (var day in listGiorni)
            {
                //Corsi generale, utenza + corsi
				string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.FacoltaId, 0, day.DateString);
				List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, day.Data);

				updateSingleCorso (_db, listaCorsi);

                //CHECK USO UTENZA
                //string s_ut = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.Facolta, 0, d.DateString);
                //List<CorsoGiornaliero> listaUtenze = Web.GetSingleOrarioGiornaliero(s, 0, d.Data);

                //foreach (var u in listaUtenze)
                //{
                //    var utenza = u;
                //    if (utenza.Insegnamento.Contains("Utenza"))
                //        _db.Insert(new Utenza() { Data = utenza.Date, Aulaora = utenza.AulaOra });
                //}
            }

            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();

        }

        private void loadListCorsiGiorno()
        {
			var orari = _db.GetAllOrari ();
			_oggi.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_oggi.Data.Date, dateX.Date.Date) == 0);
			var oggiXXX = _oggi.ListaLezioni.Count();
			_oggi.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _oggi.Data);

			_domani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_domani.Data.Date, dateX.Date.Date) == 0);
			var domaniYYY = _oggi.ListaLezioni.Count();
			_domani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _domani.Data);

			_dopodomani.ListaLezioni = _db.GetAllOrari().OrderBy(y => y.Ora).Where(dateX => DateTime.Compare(_dopodomani.Data.Date, dateX.Date.Date) == 0);
			_dopodomani.ListUtenza = _db.GetAllUtenze().OrderBy(y => y.Ora).Where(x => x.Data == _dopodomani.Data);

			updateListaGiorni();
        }
        protected async override void OnAppearing()
        {
			checkDays ();
			updateListaGiorni ();

			if(_db.GetAllMieiCorsi().Count() > Settings.MieiCorsiCount || listGiorni[0].Data != DateTime.Today) //ne è stato aggiunto uno nuovo, è cambiato giorno
            {
                MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", true);
                await updateDbOrariUtenza();
                Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();
                MessagingCenter.Send<TabbedHomeView, bool>(this, "sync", false);
            }

			loadListCorsiGiorno();
            base.OnAppearing();
           
        }
        #endregion

        #region Event Handlers
        private void deleteMioCorso(TabbedDayView obj)
        {
			loadListCorsiGiorno();
        }
        #endregion
    }
}
