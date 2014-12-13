using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Services;
using OrariUnibg.Views;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.Views.ViewCells;

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
        private ListView _listView;
        private Label _day;
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

            String dateString;
            DateTime date = DateTime.Now;
            if(DateTime.Now.Hour >= 19)
            {
                date = DateTime.Now.AddDays(1);
                dateString = "DOMANI - " + date.ToString("dd/MM/yyyy");
            } 
            else
                dateString = "OGGI - " + date.ToString("dd/MM/yyyy");
                

            _day = new Label()
            {
                Text = dateString,
                Font = Font.SystemFontOfSize(NamedSize.Large, FontAttributes.Bold),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            System.Diagnostics.Debug.WriteLine("DB ITEMS: " + db.GetItems().Count());
            var mieiCorsiList = db.GetItems().OrderBy(x => x.Ora).Where(dateX => dateX.Date.Day == date.Day);

            _listView = new ListView()
            {
                ItemsSource = mieiCorsiList,
                ItemTemplate = new DataTemplate(typeof(OrarioGiornCell)),
                HasUnevenRows = true,
            };

            _listView.ItemSelected += listView_ItemSelected;
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
                Spacing = 3,
                BackgroundColor = Color.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10),
                Children = { 
                    _day,
                    _listView,
                    //logo, 
                    new StackLayout() { Spacing = 5, Children={ btnGiorn, btnComp}} }
            };

            
            //var scroll = new ScrollView()
            //{
            //    Content = layout
            //};

            return layout;
        }

        void listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            DependencyService.Get<INotification>().Notify(true);
        }

        //protected override void OnAppearing()
        //{
        //    base.OnAppearing();
        //    DependencyService.Get<INotification>().Notify(true);
        //}
    }
}
