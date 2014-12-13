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


[assembly: Dependency(typeof(OrariUnibg.Droid.Services.Notifications.Notification_Android))]
namespace OrariUnibg.Droid.Services.Notifications
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
        }
    }
}