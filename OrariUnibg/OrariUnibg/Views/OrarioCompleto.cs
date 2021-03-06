﻿using System;
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
using System.Collections;
using Xamarin;

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
        private IEnumerable<IGrouping<Lezione.Day, CorsoCompletoGroupViewModel>> listaGroup;
        //private List<CorsoCompleto> OriginalList;
        private DbSQLite _db;
        private OrariCompletoViewModel _viewModel;
        private ToolbarItem tbiShowFav;
        private ToolbarItem tbiShowAll;
		private FloatingActionButtonView fab;
		private int appearingListItemIndex = 0;
		private string _listStringGroup;
//		private ToolbarItem tbiShare;
        #endregion

        #region Private Methods
        private View getView()
        {
			fab = new FloatingActionButtonView() {
				ImageName = "ic_sharee.png",
				ColorNormal = ColorHelper.Blue500,
				ColorPressed = ColorHelper.Blue900,
				ColorRipple = ColorHelper.Blue500,
				Size = FloatingActionButtonSize.Normal,
				Clicked = (sender, args) => 
				{
					share();
				}
			};

            var lblOrario = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "ORARIO COMPLETO:",
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var lblLaurea = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblLaurea.SetBinding(Label.TextProperty, "LaureaString");

            var lblAnno = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.DarkBlue,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            lblAnno.SetBinding(Label.TextProperty, "AnnoSemestre");

            lv = new ListView() 
			{ 
				HasUnevenRows = true, 
				VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorColor = Color.Transparent
			};
			lv.ItemSelected += (sender, e) => {
				((ListView)sender).SelectedItem = null;
			};

//            lv.ItemSelected += lv_ItemSelected;

            var searchbar = new SearchBar()
            {
                Placeholder = "Cerca",
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            searchbar.TextChanged += searchbar_TextChanged;


            var l = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
                Padding = new Thickness(15, 10, 15, 10),
                Spacing = 0,
                Children = { lblOrario, lblLaurea, lblAnno }
            };
            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children = 
                { 
                    l, 
                    lv, 
                    new StackLayout(){ Children = {searchbar}, BackgroundColor = Color.White}
                }
            };

            tbiShowFav = new ToolbarItem("Mostra preferiti", "ic_nostar.png", showFavourites, 0, 0);
            tbiShowAll = new ToolbarItem("Mostra tutti", "ic_star.png", showAll, 0, 0);
//			tbiShare = new ToolbarItem ("Share", "ic_next.png", share, 0, 1);

			if(Settings.SuccessLogin)
				ToolbarItems.Add(tbiShowFav);
//			ToolbarItems.Add(tbiShare);
            //ToolbarItems.Add(tbiShowAll);

			var absolute = new AbsoluteLayout() { 
				VerticalOptions = LayoutOptions.FillAndExpand, 
				HorizontalOptions = LayoutOptions.FillAndExpand };

			// Position the pageLayout to fill the entire screen.
			// Manage positioning of child elements on the page by editing the pageLayout.
			AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0f, 0f, 1f, 1f));
			absolute.Children.Add(layout);

			// Overlay the FAB in the bottom-right corner
			AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			absolute.Children.Add(fab);

			return absolute;

//            return layout;
        }
        private void setUpListView()
        {
            if (_viewModel.Group)
            {
				listaGroup = from corso in lista
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

		private string ListGroupToString()
		{
			string text = string.Format ("ORARIO COMPLETO: {0} - {1} \n\n", _viewModel.LaureaString, _viewModel.AnnoSemestre);
			var days = Enum.GetValues (typeof(OrariUnibg.Models.Lezione.Day));

			_listStringGroup = string.Empty;
			foreach (var day in days) {
				var list = listaGroup.Where (z => z.Key.ToString() == day.ToString()).SelectMany(value => value);
				if(list.Count ()>0)
					_listStringGroup += string.Format ("{0}\n{1}\n\n", day.ToString ().ToUpper (), string.Join("\n", list));
			}

			return text += _listStringGroup;
		}
        #endregion

        #region Event Handlers
		private async void share()
		{
			string text;

			var s = await DisplayActionSheet ("Condividi", "Annulla", null, "Visualizza PDF", "Condividi PDF", "Condividi Testo");
			if (s.Contains ("PDF")) { //devo creare il PDF

				if (_viewModel.Group) {
					ListGroupToString ();
					text = _listStringGroup;  //lista corsi raggruppati
				}
				else
					text = string.Join ("\n", _viewModel.ListOrari); //lista corsi
				
				PdfFile pdf = new PdfFile () { Title = "Orario completo", TitleFacolta = _viewModel.LaureaString, TitleInfo = _viewModel.AnnoSemestre, Text = text };
				pdf.CreateCompleto ();

				await pdf.Save ();
				if (s.Contains ("Condividi")) //Condividi PDF
					DependencyService.Get<IFile> ().Share (pdf._filename);
				else
					await pdf.Display (); //visualizza PDF
			} else {
				if (_viewModel.Group)
					text = ListGroupToString();
				else
					text = _viewModel.ToString ();
				text+= Settings.Firma;
				DependencyService.Get<IMethods> ().Share (text); //condividi testo

			}
				
			//Insights.Track("Share", new Dictionary <string,string>{
			//	{"Orario", "Completo_" + s},
			//});
		}

        private void showAll()
        {
			ToolbarItems.Remove (tbiShowAll);
            ToolbarItems.Add(tbiShowFav);
            lista = _viewModel.ListOrari;

            setUpListView();
        }
        private void showFavourites()
        {
//            ToolbarItems.Clear();
			ToolbarItems.Remove (tbiShowFav);
            ToolbarItems.Add(tbiShowAll);
            lista = _viewModel.ListOrari.Where(x => x.MioCorso).ToList();

            setUpListView();
        }
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

		protected override void OnAppearing()
		{
			base.OnAppearing();
			lv.ItemAppearing += List_ItemAppearing;
			lv.ItemDisappearing += List_ItemDisappearing;
		}

		protected override void OnDisappearing()
		{
			base.OnDisappearing();
			lv.ItemAppearing -= List_ItemAppearing;
			lv.ItemDisappearing -= List_ItemDisappearing;
		}

		async void List_ItemDisappearing (object sender, ItemVisibilityEventArgs e)
		{
			await Task.Run(() =>
				{
					var items = lv.ItemsSource as IList;
					if(items != null)
					{
						var index = items.IndexOf(e.Item);
						if (index < appearingListItemIndex)
						{
							Device.BeginInvokeOnMainThread(() => fab.Hide());
						}
						appearingListItemIndex = index;
					}
				});
		}

		async void List_ItemAppearing (object sender, ItemVisibilityEventArgs e)
		{
			await Task.Run(() =>
				{
					var items = lv.ItemsSource as IList;
					if(items != null)
					{
						var index = items.IndexOf(e.Item);
						if (index < appearingListItemIndex)
						{
							Device.BeginInvokeOnMainThread(() => fab.Show());
						}
						appearingListItemIndex = index;
					}
				});
		}
        #endregion
    }
}
