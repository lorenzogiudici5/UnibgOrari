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
            if (Settings.PrimoAvvio) //prima volta che avvio la app
            {
				MainPage = new TutorialView();
            }
            else
            {
				if (Settings.SuccessLogin) //login effettuto con successo
					MainPage = new MasterDetailView ();
				else //utente non login
					MainPage = new LoginView ();
            }
            
        }

        //public static Page GetMainPage()
        //{
        //    if (Settings.PrimoAvvio)
        //    {
        //        var nav = new NavigationPage(new InformationView())
        //        {
        //            BarBackgroundColor = ColorHelper.Blue700,
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
