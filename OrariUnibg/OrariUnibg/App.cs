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
            //GetString();
            //await GetString();
            var nav = new NavigationPage(new MainPage())
            {
                BarBackgroundColor = Color.FromHex("10528c"),
                BarTextColor = Color.White
            };
            return nav;
        }

    }
}
