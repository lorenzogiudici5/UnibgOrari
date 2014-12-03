using Android.App;
using Android.Content;
using Android.OS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xamarin.Forms;
using OrariUnibg.View;

[assembly: Permission(Name = "it.lorenzogiudici5.unibg.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "android.permission.WAKE_LOCK")]
[assembly: UsesPermission(Name = "it.lorenzogiudici5.unibg.permission.C2D_MESSAGE")]
[assembly: UsesPermission(Name = "com.google.android.c2dm.permission.RECEIVE")]
[assembly: UsesPermission(Name = "android.permission.GET_ACCOUNTS")]
[assembly: UsesPermission(Name = "android.permission.INTERNET")]
namespace OrariUnibg.Droid.Services
{
    //[BroadcastReceiver(Enabled = true, Exported = true)]
    //e
    //[BroadcastReceiver(Permission = "com.google.android.c2dm.permission.SEND", Enabled = true, Exported = true)]
    //[IntentFilter(new string[] { "com.google.android.c2dm.intent.RECEIVE" }, Categories = new string[] { "it.lorenzogiudici5.unibg" })]
    //[IntentFilter(new string[] { "com.google.android.c2dm.intent.REGISTRATION" }, Categories = new string[] { "it.lorenzogiudici5.unibg" })]
    //[IntentFilter(new string[] { "com.google.android.gcm.intent.RETRY" }, Categories = new string[] { "it.lorenzogiudici5.unibg" })]
    //[IntentFilter(new[] { Intent.ActionBootCompleted }, Priority = 100)]
  
    //[BroadcastReceiver]
    //[IntentFilter(new string[] { Intent.ActionBootCompleted }, Priority = Int32.MaxValue)]
    public class AlarmReceiver : BroadcastReceiver
    {
        Intent _intent;
        public override void OnReceive(Context context, Intent intent)
        {
            _intent = intent;
            PowerManager.WakeLock sWakeLock;
            var pm = PowerManager.FromContext(context);
            sWakeLock = pm.NewWakeLock(WakeLockFlags.Partial, "GCM Broadcast Reciever Tag");
            sWakeLock.Acquire();
            Console.WriteLine("\r\nreceived boot broadcast.");
            SetUpNotification();

            sWakeLock.Release();
        }

        private void SetUpNotification()
        {
            //var launchIntent = new Intent (Application.Context, typeof(MainPage));

            //var pendingIntent = PendingIntent.GetActivity(Application.Context, 0, launchIntent, PendingIntentFlags.CancelCurrent);
            Notification.Builder builder = new Notification.Builder(Forms.Context)
                .SetContentTitle(_intent.GetStringExtra("title"))
                .SetContentText("Hello World! This is my first notification!")
                .SetSmallIcon(Resource.Drawable.UnibgOk);

            // Build the notification:
            Notification notification = builder.Build();

            // Get the notification manager:
            NotificationManager notificationManager = Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            // Publish the notification:
            const int notificationId = 0;
            notificationManager.Notify(notificationId, notification);


            ////Create notification
            //var notificationManager = Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            ////Create an intent to show ui
            ////var uiIntent = new Intent(Forms.Context, typeof(SelectGiornaliero));
            
            ////Create the notification
            //var notification = new Notification(Resource.Drawable.ic_launcher, "title");
            ////var notification = new Notification(Android.Resource.Drawable.SymActionEmail,title);

            ////Auto cancel will remove the notification once the user touches it
            //notification.Flags = NotificationFlags.AutoCancel;

            ////Set the notification info
            ////we use the pending intent, passing our ui intent over which will get called
            ////when the notification is tapped.
            //notification.SetLatestEventInfo(Forms.Context,
            //    "title", "desce", PendingIntent.GetActivity(Forms.Context, 0, uiIntent, PendingIntentFlags.CancelCurrent));

            ////Show the notification
            //notificationManager.Notify(1, notification);

        }
    }
}
