using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Services;
using OrariUnibg.View;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.View.ViewCells;

namespace OrariUnibg
{
    public class MainPage : ContentPage
    {
        #region Constructor
        public MainPage()
        {
            db = new DbSQLite();
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView listView;
        private DbSQLite db;
        #endregion
        
        public Xamarin.Forms.View getView()
        {
            Title = "OrariUnibg";

            //var logo = new Image()
            //{
            //    HorizontalOptions = LayoutOptions.CenterAndExpand,
            //    VerticalOptions = LayoutOptions.CenterAndExpand,
            //    //Source = "logo_Unibg"
            //    Source = "UnibgOk.png"
            //};
            listView = new ListView()
            {
                ItemsSource = db.GetItems(),
                ItemTemplate = new DataTemplate(typeof(FavouriteCell))
            };
            var btnGiorn = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Orario Giornaliero",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
            };

            var btnComp = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Orario Completo",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
            };

            btnGiorn.Clicked += (sender, arg) =>
            {
                this.Navigation.PushAsync(new SelectGiornaliero());
            };

            btnComp.Clicked += (sender, arg) =>
            {
                this.Navigation.PushAsync(new SelectCompleto());
            };


            var layout = new StackLayout()
            {
                BackgroundColor = Color.White,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10),
                Children = { 
                    listView,
                    //logo, 
                    new StackLayout() { Spacing = 5, Children={ btnGiorn, btnComp}} }
            };

            
            var scroll = new ScrollView()
            {
                Content = layout
            };

            return scroll;
        }
    }
}
