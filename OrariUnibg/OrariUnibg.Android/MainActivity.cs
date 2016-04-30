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
using Xamarin;
using OrariUnibg.Helpers;
using ImageCircle.Forms.Plugin.Droid;

namespace OrariUnibg.Droid
{
	[Activity(Label = "UnibgOrari", ConfigurationChanges = ConfigChanges.ScreenSize | ConfigChanges.Orientation, Theme = "@style/MyTheme")] // , Theme = "@style/ActionBarCustomHeader")]
	public class MainActivity : FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle bundle)
        {
            base.OnCreate(bundle);
            global::Xamarin.Forms.Forms.Init(this, bundle);

            App.Init(new DbSQLite(new SQLite_Android().GetConnection()));

			ImageCircleRenderer.Init();
			Insights.DisableCollection = true;
//			Insights.DisableCollection = Settings.StatisticData;
			Insights.Initialize("37a1497d790f720508e527850ad82785c117c774", Xamarin.Forms.Forms.Context);

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;

            LoadApplication(new App()); // method is new in 1.3
			ToastNotificatorImplementation.Init();  //TOAST NOTIFICATION

//			if ((int)Android.OS.Build.VERSION.SdkInt >= 21) 
//			{ 
//				ActionBar.SetIcon ( new ColorDrawable (Resources.GetColor (Android.Resource.Color.Transparent))); 
//			}
		}
    }
}

