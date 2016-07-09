using OrariUnibg.Helpers;
using OrariUnibg.Services.Azure;
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
        #region Private Fields
        private static AzureDataService _service;
        #endregion

        #region Properties
        public static DbSQLite Database { get; set; }
        public static AzureDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }
        #endregion

        public App()
        {
            //***DEBUG NO AUTENTICAZIONE
            //MainPage = new NavigationPage(
            //    new MasterDetailView())
            //{
            //    BarBackgroundColor = ColorHelper.Blue700,
            //    BarTextColor = ColorHelper.White
            //};

            _service = new AzureDataService();

            if (Settings.PrimoAvvio) //prima volta che avvio la app
            {
                MainPage = new TutorialView() { Service = _service };
            }
            else
            {
                if (Settings.IsLoggedIn) //login effettuto con successo
                {
                    Settings.ToUpdate = false;
                    MainPage = new MasterDetailView() { Service = _service };
                }

                else //utente non login
                    MainPage = new LoginView() { Service = _service };
            }
        }



        public static void Init(DbSQLite sqlite)
        {
            Database = sqlite;
        }

    }
}
