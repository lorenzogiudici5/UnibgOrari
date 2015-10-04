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
using OrariUnibg.Services.Database;
using Android.Graphics.Drawables;
using Toasts.Forms.Plugin.Droid;

namespace OrariUnibg.Droid
{
	[Activity(Label = "OrariUniBg", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Theme = "@style/MyTheme")] // , Theme = "@style/ActionBarCustomHeader")]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsApplicationActivity // superclass new in 1.3
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

           

            //Xamarin.Forms.Forms.Init(this, bundle);

            //App.Init(new DbSQLite());
            App.Init(new DbSQLite(new SQLite_Android().GetConnection()));

            //SetPage(App.GetMainPage());
            LoadApplication(new App()); // method is new in 1.3
			ToastNotificatorImplementation.Init();  //TOAST NOTIFICATION

			if ((int)Android.OS.Build.VERSION.SdkInt >= 21) 
			{ 
				ActionBar.SetIcon ( new ColorDrawable (Resources.GetColor (Android.Resource.Color.Transparent))); 
			}
		}
    }
}

