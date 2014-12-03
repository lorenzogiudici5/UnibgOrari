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
        //static string s = "TEST";
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
            //return new ContentPage
            //{
            //    Content = new Label
            //    {
            //        Text = "TEST",
            //        VerticalOptions = LayoutOptions.CenterAndExpand,
            //        HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    },
            //};
        }

        //public static void GetString()
        //{
        //    s = Web.GetWebString();
        //}

    }
}
