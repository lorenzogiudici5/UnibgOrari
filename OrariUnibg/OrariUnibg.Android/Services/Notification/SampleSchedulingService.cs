﻿using Android.App;
using Android.Content;
using Android.Support.V4.App;
using OrariUnibg.Models;
using OrariUnibg.Services.Database;
using OrariUnibg.View;
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
        private DbSQLite db;
        private Intent _intent;
        public SampleSchedulingService() { }
        protected override async void OnHandleIntent(Android.Content.Intent intent)
        {
            db = new DbSQLite();
            System.Diagnostics.Debug.WriteLine("START SERVICE");
            _intent = intent;
            DateTime date;

            if (DateTime.Now.Hour > 18)
                date = DateTime.Now.AddDays(1);
            else
                date = DateTime.Now;
            
            string s = await Web.GetOrarioGiornaliero("IN", 1, 19, date.ToString("dd'/'MM'/'yyyy"));
            List<CorsoGiornaliero> lista = Web.GetSingleOrarioGiornaliero(s, 0);
            
            /*
            * lista dei corsi salvati. Scandisco ogni elemento della lista con la lista di tutti i corsi di giornata
            * se insegnamento+ora sono uguali, verifico se le Note cambiano. Se cambiano, la salvo nel database e invio la notifica
            * */
            
            foreach (var l in lista)
            {
                foreach(var x in db.GetItems())
                {
                    if (l.Insegnamento == x.Insegnamento)
                    {
                        x.
                        SendNotification(l);
                    }
                }

            }

            // Release the wake lock provided by the BroadcastReceiver.
            SampleAlarmReceiver.CompleteWakefulIntent(intent);
        }

        private void SendNotification(CorsoGiornaliero l)
        {
            System.Diagnostics.Debug.WriteLine("SEND NOTIFICATION");

            // Set up an intent so that tapping the notifications returns to this app:
            Intent intent = new Intent (this, typeof(MainActivity));

            // Create a PendingIntent; we're only using one PendingIntent (ID = 0):
            const int pendingIntentId = 0;
            PendingIntent pendingIntent = PendingIntent.GetActivity (this, pendingIntentId, intent, PendingIntentFlags.UpdateCurrent);

            Notification.Builder builder = new Notification.Builder(this)
                .SetContentIntent(pendingIntent)
                .SetContentTitle(l.Note.ToUpper())
                .SetContentText(l.Insegnamento + " - " + l.Ora)
                .SetSmallIcon(Resource.Drawable.UnibgOk);
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
            const int notificationId = 1;
            notificationManager.Notify(notificationId, notification);
        }


    }
}
