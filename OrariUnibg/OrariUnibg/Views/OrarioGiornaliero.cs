using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.ViewModels;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views
{
    class OrarioGiornaliero : ContentPage
    {
        #region Constructor
        public OrarioGiornaliero()
        {
            _db = new DbSQLite();
            this.SetBinding(ContentPage.TitleProperty, "FacoltaString");

            Content = getView();
        }

        #endregion
        #region Private Fields
        private DbSQLite _db;
        private ListView lv;
        private OrariGiornoViewModel _viewModel;
        private List<CorsoGiornaliero> _listOriginal;
        private ToolbarItem tbiShowFav;
        private ToolbarItem tbiShowAll;
        #endregion

        #region Private Methods

        private View getView()
        {
            var lblData = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblData.SetBinding(Label.TextProperty, "DataString");

            var lblLaurea = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblLaurea.SetBinding(Label.TextProperty, "LaureaString");

            lv = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioGiornCell)),
                HasUnevenRows = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
            };
            lv.SetBinding(ListView.ItemsSourceProperty, "ListOrari");
            lv.ItemSelected += lv_ItemSelected;
			lv.SeparatorColor = Color.Transparent;

            var searchbar = new SearchBar()
            {
                Placeholder = "Cerca",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            //searchbar.SearchButtonPressed += searchbar_SearchButtonPressed;
            searchbar.TextChanged += searchbar_TextChanged;

            var layoutLabel = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
                Spacing = 0,
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children = { lblData, lblLaurea}
            };

            var l = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children = 
                { 
                    layoutLabel, 
                    lv, 
                    new StackLayout(){ Children = {searchbar}, BackgroundColor = Color.White} 
                }
            };

            tbiShowFav = new ToolbarItem("Mostra preferiti", "ic_nostar.png", showFavourites, 0, 0);
            tbiShowAll = new ToolbarItem("Mostra tutti", "ic_star.png", showAll, 0, 0);

            ToolbarItems.Add(tbiShowFav);
            //ToolbarItems.Add(tbiShowAll);

            return l;

        }

        async void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var orario = (CorsoGiornaliero)lv.SelectedItem;

            //if(il numero di facoltà presa dal ViewModel è uguale a Settings.Facolta allora displasy actionsheet) sennò vai direttamente alla pagina del corso
            if(_viewModel.Facolta.IdFacolta == Settings.FacoltaId)
            {
                string action;
                if (_db.CheckAppartieneMieiCorsi(orario))
                    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Rimuovi dai preferiti");
                else
                    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Aggiungi ai preferiti");

                switch (action)
                {
                    case "Rimuovi dai preferiti":
                        var conferma = await DisplayAlert("RIMUOVI", string.Format("Sei sicuro di volere rimuovere {0} dai corsi preferiti?", orario.Insegnamento), "Conferma", "Annulla");
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
                        await DisplayAlert("PREFERITO AGGIUNTO!", string.Format("Complimenti! Hai aggiunto il corso di {0} ai preferiti!", orario.Insegnamento), "OK");
                        break;
                    case "Dettagli":
                        //var nav = new DettagliCorsoView();
                        ////nav.BindingContext = 
                        //await this.Navigation.PushAsync(nav);
                        break;
                }
            }
            

            //MessagingCenter.Send<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", orario);

            ((ListView)sender).SelectedItem = null;
        }

        #endregion

        #region Event Handlers

        private void showAll()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(tbiShowFav);
            _viewModel.ListOrari = _listOriginal;
        }
        private void showFavourites()
        {
            ToolbarItems.Clear();
            ToolbarItems.Add(tbiShowAll);
            _viewModel.ListOrari = _viewModel.ListOrari.Where(x => x.MioCorso).ToList();
        }
        void searchbar_TextChanged(object sender, TextChangedEventArgs e)
        {
            SearchBar searchBar = (SearchBar)sender;
            string searchText = searchBar.Text.ToUpper();

            if (searchText == string.Empty)
                //lv.ItemsSource = _listOriginal;
                _viewModel.ListOrari = _listOriginal;
            else
                _viewModel.ListOrari = _viewModel.ListOrari.Where(x => x.Insegnamento.Contains(searchText) || x.Docente.Contains(searchText) || x.AulaOra.ToUpper().Contains(searchText) || x.Note.ToUpper().Contains(searchText)).ToList();
            
        }
        #endregion

        #region Overrides
        protected override void OnBindingContextChanged()
        {
            base.OnBindingContextChanged();
            _viewModel = (OrariGiornoViewModel)BindingContext;

            _listOriginal = _viewModel.ListOrari;
        }
        #endregion

    }
}
