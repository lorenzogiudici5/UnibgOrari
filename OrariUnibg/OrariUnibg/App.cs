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
    public class App : Application
    {
        public App()
        {
//            if (Settings.PrimoAvvio)
//            {
//                var nav = new NavigationPage(new InformationView())
//                {
//                    BarBackgroundColor = ColorHelper.Blue,
//                    BarTextColor = ColorHelper.White
//                };
//                MainPage = nav;
//            }
//            else
//            {
//                MainPage = new MasterDetailView();
//            }
//            
			MainPage = new MasterDetailView();
            
        }

        //public static Page GetMainPage()
        //{
        //    if (Settings.PrimoAvvio)
        //    {
        //        var nav = new NavigationPage(new InformationView())
        //        {
        //            BarBackgroundColor = ColorHelper.Blue,
        //            BarTextColor = ColorHelper.White
        //        };
        //        return nav;
        //    }
        //    else
        //        return new MasterDetailView();
        //}

        public static void Init(DbSQLite sqlite)
        {
            Database = sqlite;
        }

        public static DbSQLite Database { get; set; }

    }
}
