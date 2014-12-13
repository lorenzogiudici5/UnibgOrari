using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class InformationView : ContentPage
    {
        #region Constructor
        public InformationView()
        {
            Title = "Informazioni";
            Content = getView();
        }      
        #endregion

        #region Private Fields
        private Entry _entryNome;
        private Entry _entryCognome;
        private Picker _pickFacolta;
        private Picker _pickLaurea;
        private Label _lblNotific;
        private Switch _switchNotific;
        private Label _lblSync;
        private Switch _switchSync;
        #endregion

        #region Private Methods
        private View getView()
        {
            _entryNome = new Entry()
            {
                Placeholder = "Nome",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            _entryCognome = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Cognome"
            };

            _pickFacolta = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Facoltà"
            };
            _pickLaurea = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Laurea"
            };

            _lblNotific = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Notifiche variazioni lezioni"
            };
            _switchSync = new Switch();
            _lblSync = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Sincronizzazione bakgruound"
            };
            _switchNotific = new Switch();

            var grid = new Grid()
            {
                Padding = new Thickness(15, 10, 15, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = 5,
                ColumnSpacing = 5,
                RowDefinitions = 
                {
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions = 
                { 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                }
            };
           
            grid.Children.Add(_entryNome, 0, 2, 0, 1);
            grid.Children.Add(_entryCognome, 0, 2, 1, 2);
            grid.Children.Add(_pickFacolta, 0, 2, 2, 3);
            grid.Children.Add(_pickLaurea, 0, 2, 3, 4);
            grid.Children.Add(_lblSync, 0, 1, 4, 5);
            grid.Children.Add(_switchSync, 1, 2, 4, 5);
            grid.Children.Add(_lblNotific, 0, 1, 5, 6);
            grid.Children.Add(_switchNotific, 1, 2, 5, 6);

            ToolbarItem tbiNext = new ToolbarItem("Avanti", "ic_menu.png", () =>
                {
                    Navigation.PushModalAsync(new MasterDetailView());
                    //Navigation.PopModalAsync();
                }, 0, 0); 
            //if (Device.OS == TargetPlatform.Android)
            //{ // BUG: Android doesn't support the icon being null
            //    tbiNext = new ToolbarItem("Avanti", "ic_menu.png", () =>
            //    {
            //        Navigation.PushAsync(new MasterDetailView());
            //    }, 0, 0);
            //}
            //if (Device.OS == TargetPlatform.WinPhone)
            //{
            //    tbi = new ToolbarItem("Avanti", "ic_menu.png", () =>
            //    {
            //        Navigation.PushAsync(new MasterDetailView());
            //    }, 0, 0);
            //}
            ToolbarItems.Add(tbiNext);
            return grid;
        }
        #endregion
    }
}
