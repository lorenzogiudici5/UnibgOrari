using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.V4.App;
using OrariUnibg.Droid.Services.Database;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Xamarin.Forms;
using Plugin.Connectivity;
using Plugin.Toasts;

namespace OrariUnibg.Droid.Services.Notifications
{
    [Service]
    public class SampleSchedulingService : IntentService
    {
        #region Private Fields
        private DbSQLite _db;
        private Intent _intent;
        private Giorno _oggi;
        private Giorno _domani;
        private Giorno _dopodomani;
        private IEnumerable<Orari> _listOrariGiorno;
        #endregion 

//		public SampleSchedulingService() : base("SchedulingIntentService")
//		{ }

        protected async override void OnHandleIntent(Android.Content.Intent intent)
        {
			Logcat.Write("ALARM: " + DateTime.Now.ToLongTimeString());

            _db = App.Database;

            if (_db == null) {
				App.Init (new DbSQLite (new SQLite_Android ().GetConnection ()));
				_db = App.Database;
//				if (_db == null)
//					Logcat.Write ("_db ancora NULL dopo INIT");
//				else
//					Logcat.Write ("_db NOT NULL dopo INIT");
			} else
                Logcat.Write("_db NOT NULL");

            Logcat.WriteDB(_db, "Allarme attivato");
            Logcat.Write("NUMERO MIEI CORSI: " + _db.GetAllMieiCorsi().Count());

            _intent = intent;
            _listOrariGiorno = _db.GetAllOrari();

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
            
            /*
            * lista dei corsi salvati. Scandisco ogni elemento della lista con la lista di tutti i corsi di giornata
            * se insegnamento+ora sono uguali, verifico se le Note cambiano. Se cambiano, la salvo nel database e invio la notifica
            * */
            await updateDbOrariUtenza();

            Logcat.WriteDB(_db, "AGGIORNAMENTO COMPLETATO!");
            Logcat.Write("AGGIORNAMENTO COMPLETATO!");
            // Release the wake lock provided by the BroadcastReceiver.
            SampleAlarmReceiver.CompleteWakefulIntent(intent);
        }

        #region Private Methods
        private async Task updateDbOrariUtenza()
        {
            DateTime[] arrayDate = new DateTime[] { _oggi.Data, _domani.Data, _dopodomani.Data };

            var _listOrariGiorno = _db.GetAllOrari(); //Elimina gli orari già passati
            
			foreach (var l in _listOrariGiorno)
            {
                if (l.Date < _oggi.Data)
                    _db.DeleteSingleOrari(l.Id);
            };

			if (!CrossConnectivity.Current.IsConnected) { //non connesso a internet
                Logcat.WriteDB(_db, "*************ERRORE");
                Logcat.WriteDB(_db, "AGGIORNAMENTO NON RIUSCITO, nessun accesso a internet");
                Logcat.Write("AGGIORNAMENTO NON RIUSCITO, nessun accesso a internet");
                //var toast = DependencyService.Get<IToastNotificator>();
				//await toast.Notify (ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds (3));
				return;
			}
			
            foreach (var d in arrayDate)
            {
				Logcat.Write ("Data Considerata: " + d.ToString());
                Logcat.WriteDB(_db, "Ottenimento orari del " + d.Date.ToString());

                string s = await Web.GetOrarioGiornaliero(Settings.FacoltaDB, Settings.FacoltaId, 0, d.ToString("dd'/'MM'/'yyyy"));
				List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

				if (listaCorsi.Count () != 0)
					updateSingleCorso (listaCorsi);
                Logcat.WriteDB(_db, "Ottenimento orari del " + d.Date.ToString() + " OK");
            }

            Settings.MieiCorsiCount = _db.GetAllMieiCorsi().Count();

        }

		private void updateSingleCorso(List<CorsoGiornaliero> listaCorsi)
		{
			foreach (var l in listaCorsi)
			{
				var corso = l;
				if (_db.CheckAppartieneMieiCorsi(l))
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
						var o = _db.GetAllOrari().FirstOrDefault(y => y.Insegnamento == orario.Insegnamento && y.Date == orario.Date);
						if ((string.Compare(o.Note, corso.Note) != 0) || !o.Notify) //ci sono state notifiche non ancora segnalate
						{
							o.Note = corso.Note;
							o.AulaOra = corso.AulaOra;
							if (o.Note != null && o.Note != "" && !o.Notify)
							{
                                Logcat.Write("---------- - Invio Notifica: " + o.Insegnamento);
                                Logcat.WriteDB(_db, "-----------Invio Notifica: " + o.Insegnamento);
                                SendNotification(corso);
                                Logcat.Write("-----------Invio Notifica OK");
                                Logcat.WriteDB(_db, "-----------Invio Notifica OK");
                                o.Notify = true;
							}
							_db.Update(o);
						}
					}
					else // l'orario non è presente nel mio db
					{
						orario.Notify = false;

						if (orario.Note != null && orario.Note != "" && !orario.Notify)
						{
//							SendNotification ("TROVATO CORSO ANNULLATO!");
							SendNotification(corso);
							orario.Notify = true;
						}

						_db.Insert(orario);
					}
				}
				else if (corso.Insegnamento.Contains("UTENZA"))
					_db.Insert(new Utenze() { Data = corso.Date, AulaOra = corso.AulaOra });
			}		

			Settings.LastUpdate = DateTime.Now.ToString ("dd/MM/yyyy HH:mm:ss");
		}
		public void SendNotification(CorsoGiornaliero l)
		{
			if (!Settings.Notify)
				return;
            //			Logcat.Write("SEND NOTIFICATION");

            Logcat.Write("Creazione Notifica");
            Logcat.WriteDB(_db, "Creazione notifica");
            Logcat.WriteDB(_db, l.Note.ToUpper() + l.Insegnamento + " - " + l.Date + " - " + l.Ora);
            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(Forms.Context, typeof(MainActivity));

			// Create a PendingIntent; we're only using one PendingIntent (ID = 0):
			const int pendingIntentId = 0;
			PendingIntent pendingIntent = PendingIntent.GetActivity(Forms.Context, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

			Notification.Builder builder = new Notification.Builder(Forms.Context)
				.SetContentIntent(pendingIntent)
				.SetContentTitle(l.Note.ToUpper())
				.SetContentText(l.Insegnamento + " - " + l.Date + " - " + l.Ora )
				.SetSmallIcon(Resource.Drawable.ic_notification_school)
				.SetAutoCancel(true);
			//builder.SetStyle(new Notification.BigTextStyle().BigText(longMess));

			Notification.InboxStyle inboxStyle = new Notification.InboxStyle();
			inboxStyle.AddLine(l.Insegnamento);
			inboxStyle.AddLine(l.Date.DayOfWeek + ", " + l.Date.ToShortDateString());
			inboxStyle.AddLine(l.AulaOra);
			inboxStyle.AddLine(l.Docente);
			builder.SetStyle(inboxStyle);

			// Build the notification:
			Notification notification = builder.Build();

			// Get the notification manager:
			NotificationManager notificationManager = Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

			// Publish the notification:
			//            const int notificationId = 1;
			var rnd = new System.Random ();
			notificationManager.Notify(rnd.Next(), notification);
		}

//		private void SendNotification(string text)
//		{
//			Logcat.Write("SEND NOTIFICATION");
//
//			// Set up an intent so that tapping the notifications returns to this app:
//			Intent intent = new Intent(this, typeof(MainActivity));
//
//			// Create a PendingIntent; we're only using one PendingIntent (ID = 0):
//			const int pendingIntentId = 0;
//			PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);
//
//			Notification.Builder builder = new Notification.Builder(this)
//				.SetContentIntent(pendingIntent)
//				.SetContentTitle("NOTIFICA")
//				.SetContentText(text)
//				.SetSmallIcon(Resource.Drawable.UnibgOk);
//
//			Notification.InboxStyle inboxStyle = new Notification.InboxStyle();
//			builder.SetStyle(inboxStyle);
//
//			// Build the notification:
//			Notification notification = builder.Build();
//
//			// Get the notification manager:
//			NotificationManager notificationManager = this.GetSystemService(Context.NotificationService) as NotificationManager;
//
//			// Publish the notification:
//			int notificationId = new Random().Next();
//			notificationManager.Notify(notificationId, notification);
//		}
//
//        private void SendNotification()
//        {
//			Logcat.Write("SEND NOTIFICATION");
//
//            // Set up an intent so that tapping the notifications returns to this app:
//            Intent intent = new Intent(this, typeof(MainActivity));
//
//            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
//            const int pendingIntentId = 0;
//            PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);
//
//            Notification.Builder builder = new Notification.Builder(this)
//                .SetContentIntent(pendingIntent)
//                .SetContentTitle("NOTIFICA")
//				.SetContentText("NOTIFICA TEST" + DateTime.Now)
//                .SetSmallIcon(Resource.Drawable.UnibgOk);
//
//            Notification.InboxStyle inboxStyle = new Notification.InboxStyle();
//            builder.SetStyle(inboxStyle);
//
//            // Build the notification:
//            Notification notification = builder.Build();
//
//            // Get the notification manager:
//            NotificationManager notificationManager = this.GetSystemService(Context.NotificationService) as NotificationManager;
//
//            // Publish the notification:
//            const int notificationId = 1;
//            notificationManager.Notify(notificationId, notification);
//        }
        #endregion


    }
}
