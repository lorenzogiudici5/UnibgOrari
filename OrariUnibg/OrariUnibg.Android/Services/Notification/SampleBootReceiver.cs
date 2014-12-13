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


namespace OrariUnibg.Droid.Services.Notifications
{
    [BroadcastReceiver(Enabled = false)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class SampleBootReceiver : BroadcastReceiver
    {
        SampleAlarmReceiver alarm = new SampleAlarmReceiver();

        public override void OnReceive(Context context, Intent intent)
        {
            System.Diagnostics.Debug.WriteLine("BOOT RECEIVER ON RECEIVE");
            alarm.SetAlarm(context);

            if (intent.Action.Equals("android.intent.action.BOOT_COMPLETED"))
            {
                System.Diagnostics.Debug.WriteLine("ACTION EQUALS BOOT COMPLTED");
                alarm.SetAlarm(context);
            }
        }
    }
}