using OrariUnibg.Helpers;
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

            //return new MasterDetailView();
            var nav = new NavigationPage(new InformationView())
            {
                BarBackgroundColor = ColorHelper.Blue,
                BarTextColor = ColorHelper.White
            };
            return nav;
            
        }

    }
}
