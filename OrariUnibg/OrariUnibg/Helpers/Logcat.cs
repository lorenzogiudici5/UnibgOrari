using System;

namespace OrariUnibg
{
	public static class Logcat
	{
		private const string TAG = "ORARI_UNIBG: ";

		public static void Write(object log)
		{
			System.Diagnostics.Debug.WriteLine (TAG + log);
		}
	}
}

