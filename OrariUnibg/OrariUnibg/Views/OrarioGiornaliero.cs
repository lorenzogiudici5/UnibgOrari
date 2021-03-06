﻿using System;
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
using System.Collections;
using Xamarin;
using OrariUnibg.Services;

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
		private FloatingActionButtonView fab;
		private int appearingListItemIndex = 0;
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
//				ColorNormal = Color.FromHex("ff3498db"),
//				ColorPressed = Color.Black,
//				ColorRipple = Color.FromHex("ff3498db")
			};

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
				SeparatorColor = Color.Transparent
            };
            lv.SetBinding(ListView.ItemsSourceProperty, "ListOrari");
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
			AbsoluteLayout.SetLayoutFlags(l, AbsoluteLayoutFlags.All);
			AbsoluteLayout.SetLayoutBounds(l, new Rectangle(0f, 0f, 1f, 1f));
			absolute.Children.Add(l);

			// Overlay the FAB in the bottom-right corner
			AbsoluteLayout.SetLayoutFlags(fab, AbsoluteLayoutFlags.PositionProportional);
			AbsoluteLayout.SetLayoutBounds(fab, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
			absolute.Children.Add(fab);


//            return l;
			return absolute;

        }


        #endregion

        #region Event Handlers

		private async void share()
		{
			string text = _viewModel.ToString ();

			var s = await DisplayActionSheet ("Condividi", "Annulla", null, "Visualizza PDF", "Condividi PDF", "Condividi Testo");
			if (s.Contains("PDF")) {
				PdfFile pdf = new PdfFile () { Title = "Orario Giornaliero", TitleFacolta = _viewModel.LaureaString, TitleInfo = _viewModel.DataString, Text = string.Join("\n", _viewModel.ListOrari) };
				pdf.CreateGiornaliero ();

				await pdf.Save ();
				if(s.Contains("Condividi")) //Condividi PDF
					DependencyService.Get<IFile> ().Share (pdf._filename);
				else
					await pdf.Display (); //visualizza PDF
			} 
			else{
				text+= Settings.Firma;
				DependencyService.Get<IMethods> ().Share (text); //condividi testo
			}
				
			//Insights.Track("Share", new Dictionary <string,string>{
			//	{"Orario", "Giornaliero_" + s},
			//});

		}
        private void showAll()
        {
//            ToolbarItems.Clear();
			ToolbarItems.Remove(tbiShowAll);
            ToolbarItems.Add(tbiShowFav);
            _viewModel.ListOrari = _listOriginal;
        }
        private void showFavourites()
        {
//            ToolbarItems.Clear();
			ToolbarItems.Remove(tbiShowFav);
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
