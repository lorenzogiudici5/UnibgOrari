using Android.App;
using Android.Content;
using Android.Net;
using Android.Support.V4.App;
using OrariUnibg.Droid.Services.Database;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Services.Database;
using OrariUnibg.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;

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

        public SampleSchedulingService() { }
        protected override void OnHandleIntent(Android.Content.Intent intent)
        {
            System.Diagnostics.Debug.WriteLine("CREATING DB");
            //SendNotification();
            //App.Database = new DbSQLite(new SQLite_Android().GetConnection());
            _db = App.Database;
            System.Diagnostics.Debug.WriteLine("START SERVICE");
            System.Diagnostics.Debug.WriteLine("NUMERO MIEI CORSI: " + _db.GetAllMieiCorsi().Count());
            _intent = intent;
            _listOrariGiorno = _db.GetAllOrari();

            //_mieiCorsiList = db.GetItemsMieiCorsi();
            if (DateTime.Now.Hour < 18)
            {
                _oggi = new Giorno() { Data = DateTime.Today };
                _domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
                _dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
            }
            else
            {
                _oggi = new Giorno() { Data = DateTime.Today.AddDays(1) };
                _domani = new Giorno() { Data = _oggi.Data.AddDays(1) };
                _dopodomani = new Giorno() { Data = _domani.Data.AddDays(1) };
            }
            
            /*
            * lista dei corsi salvati. Scandisco ogni elemento della lista con la lista di tutti i corsi di giornata
            * se insegnamento+ora sono uguali, verifico se le Note cambiano. Se cambiano, la salvo nel database e invio la notifica
            * */
            updateDbOrariUtenza();


            // Release the wake lock provided by the BroadcastReceiver.
            SampleAlarmReceiver.CompleteWakefulIntent(intent);
        }


        #region Private Methods
        private async void updateDbOrariUtenza()
        {
            DateTime[] arrayDate = new DateTime[] { _oggi.Data, _domani.Data, _dopodomani.Data };

            var _listOrariGiorno = _db.GetAllOrari(); //Elimina gli orari già passati
            foreach (var l in _listOrariGiorno)
            {
                if (l.Date < _oggi.Data)
                    _db.DeleteSingleOrari(l.Id);
            };

            foreach (var d in arrayDate)
            {
                //Corsi generale, utenza + corsi
                string s = await Web.GetOrarioGiornaliero(Settings.DBfacolta, Settings.FacoltaId, 0, d.ToString("dd'/'MM'/'yyyy"));
                List<CorsoGiornaliero> listaCorsi = Web.GetSingleOrarioGiornaliero(s, 0, d);

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
                            if ((string.Compare(o.Note, corso.Note) != 0) || !o.Notify)
                            {
                                o.Note = corso.Note;
                                o.AulaOra = corso.AulaOra;
                                if (o.Note != null && o.Note != "" && !o.Notify)
                                {
                                    SendNotification(corso);
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
                                SendNotification(corso);
                                orario.Notify = true;
                            }

                            _db.Insert(orario);
                        }
                    }
                    else if (corso.Insegnamento.Contains("UTENZA"))
                        _db.Insert(new Utenza() { Data = corso.Date, AulaOra = corso.AulaOra });
                }

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
        private void SendNotification(CorsoGiornaliero l)
        {
            if (!Settings.Notify)
                return;

            var connectivityManager = (ConnectivityManager)GetSystemService(ConnectivityService);
            var activeConnection = connectivityManager.ActiveNetworkInfo;
            if ((activeConnection == null) || !activeConnection.IsConnected)
                return; //rete non disponibilie

            System.Diagnostics.Debug.WriteLine("SEND NOTIFICATION");

            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent(this, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            Notification.Builder builder = new Notification.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(l.Note.ToUpper())
                .SetContentText(l.Insegnamento + " - " + l.Ora)
                .SetSmallIcon(Resource.Drawable.ic_notification_school)
                .SetAutoCancel(true);
            //builder.SetStyle(new Notification.BigTextStyle().BigText(longMess));

            Notification.InboxStyle inboxStyle = new Notification.InboxStyle();
            inboxStyle.AddLine(l.Insegnamento);
            inboxStyle.AddLine(l.AulaOra);
            inboxStyle.AddLine(l.Docente);
            builder.SetStyle(inboxStyle);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = this.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            int notificationId = new Random().Next();
            //const int notificationId = 1;
            notificationManager.Notify(notificationId, notification);
        }

        //private void SendNotification()
        //{
        //    System.Diagnostics.Debug.WriteLine("SEND NOTIFICATION");

        //    // Set up an intent so that tapping the notifications returns to this app:
        //    Intent intent = new Intent(this, typeof(MainActivity));

        //    // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
        //    const int pendingIntentId = 0;
        //    PendingIntent pendingIntent = PendingIntent.GetActivity(this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

        //    Notification.Builder builder = new Notification.Builder(this)
        //        .SetContentIntent(pendingIntent)
        //        .SetContentTitle("NOTIFICA")
        //        .SetContentText("NOTIFICA TEST")
        //        .SetSmallIcon(Resource.Drawable.UnibgOk);
        //    //builder.SetStyle(new Notification.BigTextStyle().BigText(longMess));

        //    Notification.InboxStyle inboxStyle = new Notification.InboxStyle();
        //    //inboxStyle.AddLine(l.Insegnamento);
        //    //inboxStyle.AddLine(l.AulaOra);
        //    //inboxStyle.AddLine(l.Docente);
        //    builder.SetStyle(inboxStyle);

        //    // Build the notification:
        //    Notification notification = builder.Build();

        //    // Get the notification manager:
        //    NotificationManager notificationManager = this.GetSystemService(Context.NotificationService) as NotificationManager;

        //    // Publish the notification:
        //    const int notificationId = 1;
        //    notificationManager.Notify(notificationId, notification);
        //}
        #endregion


    }
}
