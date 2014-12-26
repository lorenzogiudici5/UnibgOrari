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

            var homeNav = new NavigationPage(new TabbedHomeView())
            {
                BarBackgroundColor = ColorHelper.Blue,
                BarTextColor = ColorHelper.White
            };
            Detail = homeNav;
            IsPresented = false;

            pages.Add(MenuType.Home, homeNav);

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
                        BarBackgroundColor = ColorHelper.Blue,
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
            BindingContext = viewModel;
            Icon = "ic_menu.png";
            //this.Icon = "ic_navigation_drawer.png";
            var _lblUtente = new Label()
            {
                Text = string.Format("{0} {1}", Settings.Nome, Settings.Cognome),
                TextColor = ColorHelper.White,
            };
            var _lblMail = new Label()
            {
                Text = string.Format("{0} - {1}", Settings.Matricola, Settings.Email),
                TextColor = ColorHelper.White,
            };
            var _lblFacoltà = new Label()
            {
                Text = Settings.Facolta,
                TextColor = ColorHelper.White,
            };
            var _lblLaurea = new Label()
            {
                Text = string.Format("{0} - {1}", Settings.Laurea, Settings.Anno),
                TextColor = ColorHelper.White,
            };

            //var _lblMenu = new ContentView
            //{
            //    Padding = new Thickness(10, 20, 0, 10),
            //    BackgroundColor = Color.Transparent,
            //    Content = new Label
            //    {
            //        Text = "MENU",
            //        TextColor = Color.White,
            //        Font = Font.SystemFontOfSize(NamedSize.Medium),
            //    }
            //};

            _listView = new ListView()
            {
                ItemsSource = viewModel.MenuItems,
                ItemTemplate = new DataTemplate(typeof(MenuCell)),
                HasUnevenRows = true,
            };
            //if (mainView == null)
            //    mainView = new MainPage();
            //PageSelection = mainView;

            _listView.ItemSelected += _listView_ItemSelected;
            _listView.SelectedItem = viewModel.MenuItems[0];

            var layoutUser = new StackLayout()
            {
                Padding = new Thickness(10, 20, 0, 10),
                Spacing = 0,
                BackgroundColor = ColorHelper.Blue,
                Children = { _lblUtente, _lblMail, _lblFacoltà, _lblLaurea }
            };


            //var _lblMenu = new ContentView
            //{
            //    Padding = new Thickness(10, 20, 0, 10),
            //    BackgroundColor = Color.Transparent,
            //    Content = new Label
            //    {
            //        Text = "MENU",
            //        TextColor = Color.White,
            //        Font = Font.SystemFontOfSize(NamedSize.Medium),
            //    }
            //};

            var layout = new StackLayout 
            {
                BackgroundColor = ColorHelper.White, 
                Orientation = StackOrientation.Vertical,
                Children = { layoutUser, _listView}
            };

            Content = layout;
        }

        void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            var menuItem = _listView.SelectedItem as OrariUnibg.Models.MenuItem;
            menuType = menuItem.MenuType;
            switch (menuItem.MenuType)
            {
                case MenuType.Home:
                    if (mainView == null)
                        mainView = new TabbedHomeView();

                    PageSelection = mainView;
                    break;

                case MenuType.Giornaliero:
                    if (selectGiornView == null)
                        selectGiornView = new SelectGiornaliero();

                    PageSelection = selectGiornView;
                    break;

                case MenuType.Completo:
                    if (selectCompletoView == null)
                        selectCompletoView = new SelectCompleto();

                    PageSelection = selectCompletoView;
                    break;

                //case MenuType.Esami:
                //    if (downloadView == null)
                //        downloadView = new DownloadView();

                //    PageSelection = downloadView;
                //    break;
            }
            //Page p = PageSelection;

            //MessagingCenter.Send<MasterView, Page>(this, "menuItem_clicked", p);
        }
    }
}
