using System;
using Xamarin.Forms;
using Android.Content;

[assembly: Dependency(typeof(OrariUnibg.Droid.Services.Methods_Android))]
namespace OrariUnibg.Droid.Services
{
	public class Methods_Android : IMethods
	{
		public void Close_App()
		{
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}

		public void Share(string text)
		{
			Intent sendIntent = new Intent();
			sendIntent.SetAction(Intent.ActionSend);
			sendIntent.PutExtra(Intent.ExtraText, text);
			sendIntent.SetType("text/plain");

			Forms.Context.StartActivity(Intent.CreateChooser(sendIntent, "Condividi orario.."));
		}

//		public void Share(Uri uri)
//		{
//			Intent sendIntent = new Intent();
//			sendIntent.SetAction(Intent.ActionSend);
//			sendIntent.PutExtra(Intent.ExtraStream, uri);
//			sendIntent.SetType("application/pdf");
//
//			Forms.Context.StartActivity(Intent.CreateChooser(sendIntent, "Condividi orario.."));
//		}
	}
}

