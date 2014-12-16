using System;
using Android.Content;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;
using Android.App;
using Android.Content.PM;
using OrariUnibg.Droid.Services.Database;

namespace OrariUnibg.Droid
{
    [Activity(Label = "OrariUniBg", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation)]
    public class MainActivity : AndroidActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            Xamarin.Forms.Forms.Init(this, bundle);

            App.Init(new SQLite_Android());

            SetPage(App.GetMainPage());
        }
    }
}

