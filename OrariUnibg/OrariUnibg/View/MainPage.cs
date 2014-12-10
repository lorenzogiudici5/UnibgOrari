using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Services;
using OrariUnibg.View;
using Xamarin.Forms;

namespace OrariUnibg
{
    public class MainPage : ContentPage
    {
        Label label;
        public MainPage()
        {
            Title = "OrariUnibg";
            label = new Label();

            var logo = new Image()
            {
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                //Source = "logo_Unibg"
                Source = "UnibgOk.png"
            };

            var btnSet = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Set Alarm",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
            };        

            var btnCancel = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Cancel Alarm",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
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
                    logo, 
                    new StackLayout() { Spacing = 5, Children={ btnSet, btnCancel, btnGiorn, btnComp}} }
            };

            Content = layout;
        }
    }
}
