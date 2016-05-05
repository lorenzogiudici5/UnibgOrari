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
        private AzureDataService _service;
        #endregion
        #region Properties
        public static DbSQLite Database { get; set; }
        public AzureDataService Service
        {
            get { return _service; }
            set { _service = value; }
        }
        #endregion

        public App()
        {
            _service = new AzureDataService();

            if (Settings.PrimoAvvio) //prima volta che avvio la app
            {
                MainPage = new TutorialView() { Service = _service };
            }
            else
            {
                if (Settings.IsLoggedIn) //login effettuto con successo
                    MainPage = new MasterDetailView() { Service = _service };
                else //utente non login
                    MainPage = new LoginView() { Service = _service };
            }
//			MainPage = new SelectGiornaliero ();
        }



        public static void Init(DbSQLite sqlite)
        {
            Database = sqlite;
        }

    }
}
