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
using Android.Content.PM;
using System.Threading;
using OrariUnibg.Droid;
using System.Threading.Tasks;

namespace OrariUnibg.Droid
{
    [Activity(Label = "OrariUniBg", MainLauncher = true, NoHistory = true, Theme = "@style/Theme.Splash", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class Splash : Activity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            SetContentView(Resource.Layout.splash_layout); //layout
            //Task.Delay(2000);
            //var intent = new Intent(this, typeof(MainActivity)); //creo l'intent da lanciare
            //StartActivity(intent); //faccio partire l'activity
            ThreadPool.QueueUserWorkItem(o => SlowMethod()); //metodo per addormentare la schermata
        }
        private void SlowMethod()
        {
            Thread.Sleep(2000); //thread dorme
            var intent = new Intent(this, typeof(MainActivity)); //creo l'intent da lanciare
            StartActivity(intent); //faccio partire l'activity
        }
    }
}
