using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Xamarin.Forms;
using OrariUnibg.Services;
using Java.Util;
using OrariUnibg.Models;
using OrariUnibg.Helpers;
using System.Globalization;


[assembly: Dependency(typeof(OrariUnibg.Droid.Services.Notifications.Notification_Android))]
namespace OrariUnibg.Droid.Services.Notifications
{
    public class Notification_Android : INotification
    {
        SampleAlarmReceiver sample = new SampleAlarmReceiver();

        public void BackgroundSync()
        {
            sample.SetAlarm(Forms.Context);
            //sample.StartService(Forms.Context);
        }

        public void SendNotification(CorsoGiornaliero l)
        {
            if (!Settings.Notify)
                return;
            Logcat.Write("SEND NOTIFICATION");

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
			var day = l.Date.ToString("dddd", new CultureInfo("it-IT")).ToUpper();
			inboxStyle.AddLine(day + ", " + l.Date.ToShortDateString());
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

        //public void StartService()
        //{
        //    Intent service = new Intent(Forms.Context, typeof(SampleSchedulingService));
        //    // Start the service, keeping the device awake while it is launching.
        //}
    }
}