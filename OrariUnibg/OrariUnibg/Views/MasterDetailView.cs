using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUniBg.ViewModels;
using OrariUniBg.Views;
using OrariUniBg.Views.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;
using ImageCircle.Forms.Plugin.Abstractions;

namespace OrariUnibg.Views
{
    class MasterDetailView : MasterDetailPage
    {
        private MasterDetailViewModel ViewModel
        {
            get { return BindingContext as MasterDetailViewModel; }
        }
        
        MasterView master;
        private Dictionary<MenuType, NavigationPage> pages;

        public MasterDetailView()
        {
            pages = new Dictionary<MenuType, NavigationPage>();
            BindingContext = new MasterDetailViewModel();
            NavigationPage.SetHasNavigationBar(this, false);
            Master = master = new MasterView(ViewModel);

			NavigationPage homeNav;
			if (Settings.SuccessLogin) {
				homeNav = new NavigationPage (new TabbedHomeView ()) {
					BarBackgroundColor = ColorHelper.Blue700,
					BarTextColor = ColorHelper.White
				};

			} else {
				homeNav = new NavigationPage (new HomeSkipView ()) {
					BarBackgroundColor = ColorHelper.Blue700,
					BarTextColor = ColorHelper.White
				};

			}

            Detail = homeNav;
            IsPresented = false;
			pages.Add (MenuType.Home, homeNav);

//			App.Current.MainPage = this;

            master.PageSelectionChanged = (menuType) =>
            {
                NavigationPage newPage;
                if (pages.ContainsKey(menuType))
                {
                    newPage = pages[menuType];
                }
                else
                {
                    newPage = new NavigationPage(master.PageSelection)
                    {
                        BarBackgroundColor = ColorHelper.Blue700,
                        BarTextColor = Color.White
                    };
                    pages.Add(menuType, newPage);
                }

                Detail = newPage;
                Detail.Title = master.PageSelection.Title;
                IsPresented = false;
            };
        }

    }

    class MasterView : BaseView
    {
        #region Private Fields
        public Action<MenuType> PageSelectionChanged;
        private Page pageSelection;
        private MenuType menuType = MenuType.Home;
        private ListView _listView;
        private TabbedHomeView mainView;
        private SelectCompleto selectCompletoView;
        private SelectGiornaliero selectGiornView;
		private ImpostazioniView impostazioniView;
        private MasterDetailViewModel _viewModel;
        #endregion
        
        public Page PageSelection
        {
            get { return pageSelection; }
            set
            {
                pageSelection = value;
                if (PageSelectionChanged != null)
                    PageSelectionChanged(menuType);
            }
        }

        public MasterView(MasterDetailViewModel viewModel)
        {
            _viewModel = viewModel;
            BindingContext = viewModel;
            Icon = "ic_menu.png";
            //this.Icon = "ic_navigation_drawer.png";

//			var _imgAvatar = new CircleImage {
//				BorderColor = Color.White,
//				BorderThickness = 3,
//				HeightRequest = 65,
//				WidthRequest = 65,
//				Aspect = Aspect.AspectFill,
//				HorizontalOptions = LayoutOptions.StartAndExpand,
//				Source = "user.jpg",
//			};

            var _lblUtente = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Large, this),
                Text = string.Format("{0} {1}", Settings.Nome, Settings.Cognome),
                TextColor = ColorHelper.White,
                FontAttributes =  FontAttributes.Bold,
            };
            var _lblMail = new Label()
            {
                Text = string.Format("{0} - {1}", Settings.Email, Settings.Matricola),
                TextColor = ColorHelper.White,
            };

			var _lblAnonimo = new Label () {
				FontSize = Device.GetNamedSize(NamedSize.Large, this),
				Text = "Accesso Anonimo",
				TextColor = ColorHelper.White,
			};
            //var _lblFacoltà = new Label()
            //{
            //    Text = Settings.Facolta,
            //    TextColor = ColorHelper.White,
            //};
            //var _lblLaurea = new Label()
            //{
            //    Text = string.Format("{0} - {1}", Settings.Laurea, Settings.Anno),
            //    TextColor = ColorHelper.White,
            //};

            //var _lblMenu = new ContentView
            //{
            //    Padding = new Thickness(10, 20, 0, 10),
            //    BackgroundColor = Color.Transparent,
            //    Content = new Label
            //    {
            //        Text = "MENU",
            //        TextColor = Color.White,
            //        FontSize = Device.GetNamedSize(NamedSize.Medium, this),
            //    }
            //};

//			var layoutImg = new StackLayout () {
//				Padding = new Thickness(15, 30, 10, 15),
//				BackgroundColor = ColorHelper.Blue700,
//			};

            var layoutUser = new StackLayout()
            {
                Padding = new Thickness(15, 45, 10, 10),
                Spacing = 0,
                BackgroundColor = ColorHelper.Blue700,
//                Children = { _lblUtente, _lblMail }
            };

			if (Settings.SuccessLogin) {
//				layoutImg.Children.Add (_imgAvatar);
				layoutUser.Children.Add (_lblUtente);
				layoutUser.Children.Add (_lblMail);
			}
			else
				layoutUser.Children.Add (_lblAnonimo);


            _listView = new ListView()
            {
                ItemsSource = viewModel.MenuItems,
                ItemTemplate = new DataTemplate(typeof(MenuCell)),
                HasUnevenRows = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
				SeparatorColor = ColorHelper.Transparent,
            };
            //if (mainView == null)
            //    mainView = new MainPage();
            //PageSelection = mainView;

            _listView.ItemSelected += _listView_ItemSelected;
			_listView.SelectedItem = viewModel.MenuItems.FirstOrDefault ();

			_listView.SelectedItem = viewModel.MenuItems [0];

            var scrollview = new ScrollView()
            {
                VerticalOptions = LayoutOptions.FillAndExpand,
				BackgroundColor = ColorHelper.White, 
                Content = new StackLayout()
                {
                    Children = { _listView }
                }
            };

            var layout = new StackLayout 
            {
				BackgroundColor = ColorHelper.Blue700, 
                Orientation = StackOrientation.Vertical,
                Children = {layoutUser, scrollview}
            };

            Content = layout;
        }

        void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;

            var menuItem = _listView.SelectedItem as OrariUnibg.Models.MenuItem;
            menuType = menuItem.MenuType;

            foreach (var x in _viewModel.MenuItems)
                x.Selected = false;
			
            switch (menuItem.MenuType)
            {
	            case MenuType.Home:
	                if (mainView == null)
	                    mainView = new TabbedHomeView();

//	                _viewModel.MenuItems[0].Selected = true;
					_viewModel.MenuItems.Where(x => x.Id == 0).FirstOrDefault().Selected = true;
	                PageSelection = mainView;
	                break;

	            case MenuType.Giornaliero:
	                if (selectGiornView == null)
	                    selectGiornView = new SelectGiornaliero();

//	                _viewModel.MenuItems[1].Selected = true;
				_viewModel.MenuItems.Where(x => x.Id == 1).FirstOrDefault().Selected = true;
	                PageSelection = selectGiornView;
	                break;

	            case MenuType.Completo:
	                if (selectCompletoView == null)
	                    selectCompletoView = new SelectCompleto();

//	                _viewModel.MenuItems[2].Selected = true;
				_viewModel.MenuItems.Where(x => x.Id == 2).FirstOrDefault().Selected = true;
	                PageSelection = selectCompletoView;
	                break;

				case MenuType.Impostazioni:
					if (impostazioniView == null)
						impostazioniView = new ImpostazioniView();

//					_viewModel.MenuItems[3].Selected = true;
				_viewModel.MenuItems.Where(x => x.Id == 3).FirstOrDefault().Selected = true;
					PageSelection = impostazioniView;
					break;

                //case MenuType.Esami:
                //    if (downloadView == null)
                //        downloadView = new DownloadView();

                //    PageSelection = downloadView;
                //    break;
            }

            ((ListView)sender).SelectedItem = null;
            //Page p = PageSelection;

            //MessagingCenter.Send<MasterView, Page>(this, "menuItem_clicked", p);
        }
    }
}
