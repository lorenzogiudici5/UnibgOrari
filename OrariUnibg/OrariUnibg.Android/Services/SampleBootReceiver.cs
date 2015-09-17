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
using OrariUnibg.Services.Database;
using OrariUnibg.Droid.Services.Database;


namespace OrariUnibg.Droid.Services.Notifications
{
    [BroadcastReceiver(Enabled = false)]
    //[BroadcastReceiver(Enabled = true)]
    [IntentFilter(new[] { Intent.ActionBootCompleted })]
    public class SampleBootReceiver : BroadcastReceiver
    {
        SampleAlarmReceiver alarm = new SampleAlarmReceiver();

        public override void OnReceive(Context context, Intent intent)
        {
            Logcat.Write("CREATING APP.DATABASE");
            App.Init(new DbSQLite(new SQLite_Android().GetConnection()));

			if(App.Database == null)
				Logcat.Write("DATABASE NULLO");
			else
				Logcat.Write("DATABASE NON NULLO");

            Logcat.Write("BOOT RECEIVER ON RECEIVE");
            alarm.SetAlarm(context);

//            if (intent.Action.Equals("android.intent.action.BOOT_COMPLETED"))
//            {
//                Logcat.Write("ACTION EQUALS BOOT COMPLTED");
//                alarm.SetAlarm(context);
//            }
        }
    }
}