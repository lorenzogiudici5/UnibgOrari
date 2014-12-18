using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using OrariUnibg.Views;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg
{
    public class App
    {
        public static Page GetMainPage()
        {
            if (Settings.PrimoAvvio)
            {
                var nav = new NavigationPage(new InformationView())
                {
                    BarBackgroundColor = ColorHelper.Blue,
                    BarTextColor = ColorHelper.White
                };
                return nav;
            }
            else
                return new MasterDetailView();
        }

        //public static void Init(ISQLite sqlite)
        //{
        //    SQLite = sqlite;
        //}

        //public static ISQLite SQLite { get; private set; }

        public static void Init(DbSQLite sqlite)
        {
            Database = sqlite;
        }

        public static DbSQLite Database { get; set; }

    }
}
