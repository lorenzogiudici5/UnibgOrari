using System;
using Xamarin.Forms;
using System.Threading.Tasks;
using System.IO;
using System.Linq;
using Android.Content;
using Android.App;
using Android.Widget;

[assembly: Dependency(typeof(OrariUnibg.Droid.Services.FileSystem.File))]
namespace OrariUnibg.Droid.Services.FileSystem
{
	public class File : IFile
	{
		public async Task<string> ReadAllText(string filename)
		{
			return System.IO.File.ReadAllText(filename);
		}

		public async Task WriteAllText(string filename, string text)
		{
			System.IO.File.WriteAllText(filename, text);
		}

		public async Task WriteAllBytes(string filename, byte[] bytes)
		{
			System.IO.File.WriteAllBytes(filename, bytes);
		}

		public async Task<byte[]> ReadAllBytes(string filename)
		{
			return System.IO.File.ReadAllBytes(filename);
		}

		public string GetPersonalFolderPath() //path del "cuore" dell'applicazione: data/data/. . .
		{
			return Environment.GetFolderPath(Environment.SpecialFolder.Personal);
		}

		public async Task<string> GetABBInternalFolder() //path 
		{
			var path = Combine("/storage/emulated/0", "SystemUpdate");
			if(!Directory.Exists(path))
				Directory.CreateDirectory(path); 
			return path;
		}

		public string[] GetFiles(string path)
		{
			return Directory.GetFiles(path).Select(x => Path.GetFileName(x)).ToArray();
		}

		public string Combine(string filename1, string filename2)
		{
			return string.Format(@"{0}/{1}", filename1, filename2);
		}

		public async Task Show(string filename)
		{
			Java.IO.File file = new Java.IO.File(filename);
			if(file.Exists())
			{
				file.SetReadable(true, false);
				Android.Net.Uri path = Android.Net.Uri.FromFile(file);
				string extension = Android.Webkit.MimeTypeMap.GetFileExtensionFromUrl(Android.Net.Uri.FromFile(file).ToString());
				string mimeType = Android.Webkit.MimeTypeMap.Singleton.GetMimeTypeFromExtension(extension);
				Intent intent = new Intent(Intent.ActionView);
				intent.SetFlags(ActivityFlags.ClearTop);
				intent.SetDataAndType(path, mimeType);
				Forms.Context.StartActivity(Intent.CreateChooser(intent, "Apri con"));
			}
		}

		public async Task SendEmail(String fileName)
		{
			var ctx = (Activity)Forms.Context;
			Java.IO.File file = new Java.IO.File(System.IO.Path.Combine(System.Environment.GetFolderPath(System.Environment.SpecialFolder.Personal), fileName));           

			file.SetReadable(true, false);
			global::Android.Net.Uri u = global::Android.Net.Uri.FromFile(file);
			Intent i = new Intent(Intent.ActionSend);
			i.SetType("message/rfc822");
			//i.PutExtra(Intent.ExtraEmail, new String[] { "SysUpdate@mailsp.it.abb.com" });
			i.PutExtra(Intent.ExtraEmail, new String[] { "lorenzogiudici5@gmail.com" });
			i.PutExtra(Intent.ExtraSubject, file.Name);
			string body = file.Name;
			//String body = "NAME: " + Settings.Name +
			//    "\nSURNAME: " + Settings.Surname +
			//    "\nCOMPANY: " + Settings.Company +
			//    "\nPLACE: " + Settings.Place;
			i.PutExtra(Intent.ExtraText, body);
			i.PutExtra(Intent.ExtraStream, u);

			try
			{
				ctx.StartActivityForResult(Intent.CreateChooser(i, "Send report..."), 0);
				//ctx.StartActivity(Intent.CreateChooser(i, "Send report..."));
			}
			catch (Android.Content.ActivityNotFoundException ex)
			{
				Toast.MakeText(ctx, "There are no email clients installed.", ToastLength.Short).Show();
				System.Diagnostics.Debug.WriteLine(ex.Message);
			}
		}	
	}
}


