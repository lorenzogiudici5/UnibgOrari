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
using OrariUnibg.ViewModels;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views
{
    class OrarioCompleto : ContentPage
    {
        #region Constructor
        public OrarioCompleto()
        {
            _db = new DbSQLite();
            this.SetBinding(ContentPage.TitleProperty, "FacoltaString");
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView lv;
        private List<CorsoCompleto> lista;
        private List<CorsoCompleto> OriginalList;
        private DbSQLite _db;
        private OrariCompletoViewModel _viewModel;
        #endregion

        #region Private Methods
        private View getView()
        {
            var lblOrario = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = "ORARIO COMPLETO:",
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var lblLaurea = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblLaurea.SetBinding(Label.TextProperty, "LaureaString");

            var lblAnno = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblAnno.SetBinding(Label.TextProperty, "AnnoSemestre");

            lv = new ListView() { HasUnevenRows = true };
            lv.ItemSelected += lv_ItemSelected;

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
            var layout = new StackLayout()
            {
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Vertical,
                Children = { l, lv, searchbar }
            };

            return layout;
        }
        private void setUpListView()
        {
            if (_viewModel.Group)
            {
                var listaGroup = from corso in lista
                                 from lez in corso.Lezioni
                                 orderby lez.Ora
                                 where lez.AulaOra != string.Empty
                                 group new CorsoCompletoGroupViewModel() { Insegnamento = corso.Insegnamento, Docente = corso.Docente, Codice = corso.Codice, InizioFine = corso.InizioFine, AulaOra = lez.AulaOra, IsVisible = lez.isVisible, Giorno = lez.Giorno, Note = lez.Note, Day = lez.day } by lez.Giorno into Group
                               // group new { corso.Insegnamento, corso.Docente, Cod = corso.Codice, corso.InizioFine, lez.AulaOra, lez.Aula, lez.Ora, lez.isVisible, lez.Giorno, lez.Note, lez.day } by lez.Giorno into Group
                                 //group corso by lez.Giorno into Group
                                 select Group;

                listaGroup = listaGroup.OrderBy(x => (int)x.Key);

                lv.ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup));
                lv.ItemsSource = listaGroup;
                lv.IsGroupingEnabled = true;
                lv.GroupDisplayBinding = new Binding("Key");
                if (Device.OS != TargetPlatform.WinPhone)
                    lv.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));

                //lv.IsEnabled = false; // da eliminare quando sarà creato un view model per questa lista e quindi il cast nel click, potrà essere differenziato e valido
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

            Corso orario;
            if(_viewModel.Group)
            {
                CorsoCompletoGroupViewModel o = (CorsoCompletoGroupViewModel)lv.SelectedItem;
                orario = new Corso() { Insegnamento = o.Insegnamento, Codice = o.Codice, Docente = o.Docente };
            }
                
            else
            {
                CorsoCompleto o = (CorsoCompleto)lv.SelectedItem;
                orario = new Corso() { Insegnamento = o.Insegnamento, Codice = o.Codice, Docente = o.Docente };
            }
                
            if (_viewModel.Facolta.IdFacolta == Settings.Facolta)
            {
                string action;
                if (_db.CheckAppartieneMieiCorsi(orario))
                    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Rimuovi dai preferiti");
                else
                    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Aggiungi ai preferiti");

                switch (action)
                {
                    case "Rimuovi dai preferiti":
                        var conferma = await DisplayAlert("RIMUOVI", string.Format("Sei sicuro di volere rimuovere {0} dai corsi preferiti?", orario.Insegnamento), "Annulla", "Conferma");
                        if (conferma)
                        {
                            var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
                            _db.DeleteMieiCorsi(corso);
                        }
                        else
                            return;

                        break;
                    case "Aggiungi ai preferiti":
                        _db.Insert(new MieiCorsi() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
                        await DisplayAlert("PREFERITO AGGIUNTO!", "Complimenti! Hai aggiunto il corso di {0} ai preferiti!", "OK");
                        break;
                    default:
                    //case "Dettagli":
                    //    var nav = new DettagliCorsoView();
                    //    //nav.BindingContext = 
                    //    await this.Navigation.PushAsync(nav);
                        break;
                }
            }

            ((ListView)sender).SelectedItem = null;
        }
        #endregion

        #region Event Handlers
        void searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string searchText = searchBar.Text.ToUpper();

            if (searchText == string.Empty)
                lista = _viewModel.ListOrari;
            else
                lista = _viewModel.ListOrari.Where(x => x.Insegnamento.Contains(searchText) || x.Docente.Contains(searchText) || x.Lezioni.Any(y => y.AulaOra.ToUpper().Contains(searchText) || y.Note.ToUpper().Contains(searchText))).ToList();

            setUpListView();
        }
        #endregion

        #region Overrides
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _viewModel = (OrariCompletoViewModel)BindingContext;
            lista = _viewModel.ListOrari;
            setUpListView();
        }
        #endregion
    }
}
