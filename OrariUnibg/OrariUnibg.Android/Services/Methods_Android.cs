using System;
using Xamarin.Forms;

[assembly: Dependency(typeof(OrariUnibg.Droid.Services.Methods_Android))]
namespace OrariUnibg.Droid.Services
{
	public class Methods_Android : IMethods
	{
		public void Close_App()
		{
			Android.OS.Process.KillProcess(Android.OS.Process.MyPid());
		}
	}
}

