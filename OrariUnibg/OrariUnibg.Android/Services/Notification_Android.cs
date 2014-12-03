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


[assembly: Dependency(typeof(OrariUnibg.Droid.Services.Notification_Android))]
namespace OrariUnibg.Droid.Services
{
    public class Notification_Android : INotification
    {
        SampleAlarmReceiver sample = new SampleAlarmReceiver();
        public void Notify(bool b)
        {
            if (b)
                sample.SetAlarm(Forms.Context);
            else
                sample.cancelAlarm(Forms.Context);
            
            //Calendar now = Calendar.GetInstance(Java.Util.TimeZone.Default);
            ////Calendar alarmTime = Calendar.GetInstance(Java.Util.TimeZone.Default);
            ////alarmTime.Set(date.Year, date.Month, date.Day, date.Hour, date.Minute);
            ////alarmTime.Set(2014, 11, 24, 4, 50);
            //Calendar alarmTime = Calendar.GetInstance(Java.Util.TimeZone.Default);
            //alarmTime.Set(CalendarField.DayOfMonth, 1);
            //alarmTime.Set(CalendarField.Month, 11);
            ////alarmTime.Set(CalendarField.Year, 2014);
            //alarmTime.Set(CalendarField.Hour, 12);
            //alarmTime.Set(CalendarField.Minute, 10);
            //alarmTime.Set(CalendarField.Second, 0);
            //alarmTime.Set(CalendarField.Millisecond, 0);
            //alarmTime.Set(CalendarField.AmPm, 0);

            //AlarmManager am = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
            //Intent intent = new Intent(Application.Context, typeof(AlarmReceiver));
            ////intent.PutExtra (ALARM_ACTION, true);
            //PendingIntent pi = PendingIntent.GetBroadcast(Application.Context, 0, intent, 0);
            ////PendingIntentFlags.UpdateCurrent
            //am.SetRepeating(AlarmType.RtcWakeup, alarmTime.TimeInMillis, 10000, pi);
            //Console.WriteLine(alarmTime);

            
            //Intent intent = new Intent(Application.Context, typeof(AlarmReceiver));
            //intent.PutExtra("title", corso);
            //intent.PutExtra("message", date + " - " + corso);
            //intent.PutExtra("id", 0);
            //PendingIntent pi = PendingIntent.GetBroadcast(Application.Context, 1, intent, 0);
            //AlarmManager am = (AlarmManager)Forms.Context.GetSystemService(Context.AlarmService);
            //am.Cancel(pi);
            //am.SetRepeating(AlarmType.RtcWakeup, alarmTime.TimeInMillis, 10000, pi);
            
            //Console.WriteLine(alarmTime);

            // Instantiate the builder and set notification elements:
            //Notification.Builder builder = new Notification.Builder(Forms.Context)
            //    .SetContentTitle("Sample Notification")
            //    .SetContentText("Hello World! This is my first notification!")
            //    .SetSmallIcon(Resource.Drawable.UnibgOk);

            //// Build the notification:
            //Notification notification = builder.Build();

            //// Get the notification manager:
            //NotificationManager notificationManager = Forms.Context.GetSystemService(Context.NotificationService) as NotificationManager;

            //// Publish the notification:
            //const int notificationId = 0;
            //notificationManager.Notify(notificationId, notification);
        }
    }
}