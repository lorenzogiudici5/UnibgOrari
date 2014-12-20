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
        #region Private Fields
        private DbSQLite _db;
        private ListView lv;
        private List<CorsoGiornaliero> OriginalList;
        #endregion

        public OrarioGiornaliero(List<CorsoGiornaliero> list, String facolta, String laurea, String data)
        {
            _db = new DbSQLite();
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

        async void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var orario = (CorsoGiornaliero)lv.SelectedItem;

            string action;
            if (_db.CheckAppartieneMieiCorsi(orario))
                action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Dettagli", "Rimuovi dai preferiti");

            else
                action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Dettagli", "Aggiungi ai preferiti");

            switch (action)
            {
                case "Rimuovi dai preferiti":
                    var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
                    _db.DeleteMieiCorsi(corso);
                    break;
                case "Aggiungi ai preferiti":
                    _db.Insert(new MieiCorsi() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
                    break;
                default:
                case "Dettagli":
                    break;
            }

            //MessagingCenter.Send<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", orario);

            ((ListView)sender).SelectedItem = null;
        }

    }
}
