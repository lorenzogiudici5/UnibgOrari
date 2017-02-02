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
using Xamarin;
using OrariUnibg.Helpers;
using ImageCircle.Forms.Plugin.Droid;
using Plugin.Toasts;
using Refractored.XamForms.PullToRefresh.Droid;
using System.Threading.Tasks;
using Microsoft.Azure.Mobile;

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

            DependencyService.Register<ToastNotificatorImplementation>(); // Register your dependency
            ToastNotificatorImplementation.Init(this);  //TOAST NOTIFICATION
            PullToRefreshLayoutRenderer.Init();


            //Insights.DisableCollection = true;
            //Insights.DisableCollection = Settings.StatisticData;
            //Insights.Initialize("37a1497d790f720508e527850ad82785c117c774", Xamarin.Forms.Forms.Context);

			FormsAppCompatActivity.ToolbarResource = Resource.Layout.toolbar;
			FormsAppCompatActivity.TabLayoutResource = Resource.Layout.tabs;


            //Set up Mobile Center https://mobile.azure.com/users/lorenzogiudici5/apps/unibgorari 
            MobileCenter.Configure("99e6b366-7e20-46e7-97e9-43f3ec720529");
            LoadApplication(new App()); // method is new in 1.3


//			if ((int)Android.OS.Build.VERSION.SdkInt >= 21) 
//			{ 
//				ActionBar.SetIcon ( new ColorDrawable (Resources.GetColor (Android.Resource.Color.Transparent))); 
//			}
		}

        protected async override void OnResume()
        {
            base.OnResume();

            await Task.Delay(10);
            // Lets start from the beginning again
            //var nav = ServiceLocator.Current.GetInstance<INavService>();
            //nav.Home();
        }
    }
}

