using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class OrarioCompletoGroup : ContentPage
    {
        ListView lv;
        List<CorsoCompleto> lista;
        bool group;
        List<CorsoCompleto> OriginalList;
        public OrarioCompletoGroup(List<CorsoCompleto> list, string facolta, string laurea, string anno, string semestre, bool g)
        {
            OriginalList = list;
            lista = list;
            group = g;
            
            Title = facolta;
            var lblOrario = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = "ORARIO COMPLETO:",
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
            var lblAnno = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = anno + ": " + semestre + " semestre",
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            
            lv = new ListView(){ HasUnevenRows = true};
            lv.ItemSelected += lv_ItemSelected;
            setUpListView();

            var searchbar = new SearchBar()
            {
                Placeholder = "Cerca",
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            searchbar.TextChanged += searchbar_TextChanged;
            

            var l = new StackLayout() { 
                Padding = new Thickness(5, 5, 5, 5), 
                Spacing = 0,
                Children = { lblOrario, lblLaurea, lblAnno } };
            Content = new StackLayout()
            {
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Vertical,
                Children = { l, lv, searchbar}
            };
        }

        void searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string searchText = searchBar.Text.ToUpper();

            if (searchText == string.Empty)
                lista = OriginalList;
            else
                lista = OriginalList.Where(x => x.Insegnamento.Contains(searchText) || x.Docente.Contains(searchText) || x.Lezioni.Any(y => y.AulaOra.ToUpper().Contains(searchText) || y.Note.ToUpper().Contains(searchText))).ToList();

            setUpListView();
        }

        //void searchbar_SearchButtonPressed(object sender, EventArgs e)
        //{
        //    // Get the search text.
        //    SearchBar searchBar = (SearchBar)sender;
        //    string searchText = searchBar.Text.ToUpper();

        //    if (searchText == string.Empty)
        //        lista = OriginalList;
        //    else
        //        lista = OriginalList.Where(x => x.Insegnamento.Contains(searchText) || x.Docente.Contains(searchText)).ToList();
        //    setUpListView();
        //}

        private void setUpListView()
        {
            if (group)
            {
                var listaGroup = from corso in lista
                                 from lez in corso.Lezioni
                                 orderby lez.Ora
                                 where lez.AulaOra != string.Empty
                                 group new { corso.Insegnamento, corso.Docente, Cod = corso.Codice, corso.InizioFine, lez.AulaOra, lez.Aula, lez.Ora, lez.isVisible, lez.Giorno, lez.Note, lez.day } by lez.Giorno into Group
                                 //group corso by lez.Giorno into Group
                                 select Group;

                listaGroup = listaGroup.OrderBy(x => (int)x.Key);

                lv.ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup));
                lv.ItemsSource = listaGroup;
                lv.IsGroupingEnabled = true;
                lv.GroupDisplayBinding = new Binding("Key");
                if (Device.OS != TargetPlatform.WinPhone)
                    lv.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
            }
            else
            {
                lv.ItemsSource = lista;
                lv.ItemTemplate = new DataTemplate(typeof(OrarioComplCell));
            }


        }

        void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var x = (CorsoCompleto)lv.SelectedItem;
            DateTime date = new DateTime(2014, 11, 24, 17, 33, 0);          

            ((ListView)sender).SelectedItem = null;
        }
        
    }
}
