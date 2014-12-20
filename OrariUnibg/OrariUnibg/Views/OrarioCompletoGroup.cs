using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;
using OrariUnibg.Services.Database;

namespace OrariUnibg.Views
{
    class OrarioCompletoGroup : ContentPage
    {
        #region Constructor
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

            lv = new ListView() { HasUnevenRows = true };
            lv.ItemSelected += lv_ItemSelected;
            setUpListView();

            var searchbar = new SearchBar()
            {
                Placeholder = "Cerca",
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            searchbar.TextChanged += searchbar_TextChanged;


            var l = new StackLayout()
            {
                Padding = new Thickness(5, 5, 5, 5),
                Spacing = 0,
                Children = { lblOrario, lblLaurea, lblAnno }
            };
            Content = new StackLayout()
            {
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Vertical,
                Children = { l, lv, searchbar }
            };
        }
        #endregion

        #region Private Fields
        private ListView lv;
        private List<CorsoCompleto> lista;
        private bool group;
        private List<CorsoCompleto> OriginalList;
        private DbSQLite _db;
        #endregion

        #region Event Handlers
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
        #endregion

        #region Private Methods
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

        async void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var o = (CorsoCompleto)lv.SelectedItem;
            var orario = new CorsoGiornaliero() { Insegnamento = o.Insegnamento, Codice = o.Codice, Docente = o.Docente };
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

            ((ListView)sender).SelectedItem = null;
        }
        #endregion
    }
}
