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
using Android.Support.V4.Content;
using Java.Util;
using Xamarin.Forms;
using Android.Content.PM;
using OrariUnibg.Droid.Services.Notifications;
using OrariUnibg.Helpers;

namespace OrariUnibg.Droid.Services.Notifications
{
    [BroadcastReceiver]
    public class SampleAlarmReceiver : WakefulBroadcastReceiver
    {
        PendingIntent pi;
        AlarmManager am;
        public override void OnReceive(Context context, Intent intent)
        {
            Logcat.Write("ON RECEVICE");
            Intent service = new Intent(context, typeof(SampleSchedulingService));
            // Start the service, keeping the device awake while it is launching.
            StartWakefulService(context, service);
        }

        public void SetAlarm(Context context)
        {
            if (!Settings.BackgroundSync)
                return;
			
            Logcat.Write("SET ALARM");

            Calendar alarmTime = Calendar.GetInstance(Java.Util.TimeZone.Default);
            //alarmTime.Set(CalendarField.DayOfMonth, 1);
            //alarmTime.Set(CalendarField.Month, 11);
            //alarmTime.Set(CalendarField.Year, 2014);
            alarmTime.Set(CalendarField.Hour, 07);
            alarmTime.Set(CalendarField.Minute, 05);
            alarmTime.Set(CalendarField.Second, 0);
            alarmTime.Set(CalendarField.Millisecond, 0);
            alarmTime.Set(CalendarField.AmPm, 0);

            am = (AlarmManager)context.GetSystemService(Context.AlarmService);
            Intent intent = new Intent(context, typeof(SampleAlarmReceiver));
			Intent sampleSchedulingService = new Intent(context, typeof(SampleSchedulingService));

          pi = PendingIntent.GetBroadcast(context, 0, intent, 0);
//			var pi = PendingIntent.GetService (context, 0, sampleSchedulingService, PendingIntentFlags.CancelCurrent);
            
			//PendingIntentFlags.UpdateCurrent
//            am.SetRepeating(AlarmType.RtcWakeup, alarmTime.TimeInMillis, AlarmManager.IntervalHour, pi);

			am.SetRepeating(AlarmType.RtcWakeup, alarmTime.TimeInMillis, 70000, pi);
            Console.WriteLine(alarmTime);
			Logcat.Write("ALARM REPEATING");

            // Enable {@code SampleBootReceiver} to automatically restart the alarm when the device is rebooted.
            ComponentName receiver = new ComponentName(context, Java.Lang.Class.FromType(typeof(SampleBootReceiver)));
            PackageManager pm = context.PackageManager;

            pm.SetComponentEnabledSetting(receiver,
                ComponentEnabledState.Enabled,
                ComponentEnableOption.DontKillApp);          
        }

        public void cancelAlarm(Context context) {
            // If the alarm has been set, cancel it.
            if (am != null)
                am.Cancel(pi);

            // Disable {@code SampleBootReceiver} so that it doesn't automatically restart the 
            // alarm when the device is rebooted.
            ComponentName receiver = new ComponentName(context, Java.Lang.Class.FromType(typeof(SampleBootReceiver)));
            PackageManager pm = context.PackageManager;

            pm.SetComponentEnabledSetting(receiver,
                    ComponentEnabledState.Disabled,
                    ComponentEnableOption.DontKillApp);
        }

        public void StartService(Context context)
        {
            Intent service = new Intent(context, typeof(SampleSchedulingService));
            // Start the service, keeping the device awake while it is launching.
            StartWakefulService(context, service);
        }


    }


}