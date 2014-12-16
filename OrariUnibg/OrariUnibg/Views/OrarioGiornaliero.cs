using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;
using OrariUnibg.Services.Database;

namespace OrariUnibg.Views
{
    class OrarioGiornaliero : ContentPage
    {
        ListView lv;
        List<CorsoGiornaliero> OriginalList;
        public OrarioGiornaliero(List<CorsoGiornaliero> list, String facolta, String laurea, String data)
        {
            Title = facolta;

            OriginalList = list;
            var lblData = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = "ORARIO DEL GIORNO: " + data,
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            var lblLaurea = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = laurea.ToUpper(),
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            lv = new ListView()
            {
                ItemsSource = list,
                ItemTemplate = new DataTemplate(typeof(OrarioGiornCell)),
                HasUnevenRows = true,
            };
            lv.ItemSelected += lv_ItemSelected;

            var searchbar = new SearchBar()
            {
                Placeholder = "Cerca",
                VerticalOptions = LayoutOptions.EndAndExpand
            };

            //searchbar.SearchButtonPressed += searchbar_SearchButtonPressed;
            searchbar.TextChanged += searchbar_TextChanged;

            var layoutLabel = new StackLayout()
            {
                Spacing = 0,
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Vertical,
                Children = { lblData, lblLaurea}
            };

            var l = new StackLayout()
            {
                Padding = new Thickness(5,5,5,5),
                Orientation = StackOrientation.Vertical,
                Children = { layoutLabel, lv, searchbar }
            };

            Content = l;

        }

        void searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string searchText = searchBar.Text.ToUpper();

            if (searchText == string.Empty)
                lv.ItemsSource = OriginalList;
            else
                lv.ItemsSource = OriginalList.Where(x => x.Insegnamento.Contains(searchText) || x.Docente.Contains(searchText) || x.AulaOra.ToUpper().Contains(searchText) || x.Note.ToUpper().Contains(searchText)).ToList();
        }

        void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var x = (CorsoGiornaliero)lv.SelectedItem;

            MessagingCenter.Send<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", x);

            ((ListView)sender).SelectedItem = null;
        }

    }
}
