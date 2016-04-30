using OrariUnibg.Services.Database;
using System;

namespace OrariUnibg.Droid
{
	public static class Logcat
	{
		private const string TAG = "ORARI_UNIBG";
        public static void Write(object log)
		{
			System.Diagnostics.Debug.WriteLine (log, TAG);
		}

        public static void WriteDB(DbSQLite db, string log)
        {
            db.InsertLog(log);
        }
	}
}

