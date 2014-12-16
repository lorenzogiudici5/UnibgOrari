using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrariUniBg.Models;
using OrariUnibg.Models;

namespace OrariUniBg.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel
    {
        public ObservableCollection<MenuItem> MenuItems { get; set; }
        public MasterDetailViewModel()
        {
            Title = "Menu";
            Subtitle = "Menu";
            MenuItems = new ObservableCollection<MenuItem>();

            MenuItems.Add(new MenuItem() { Id = 0, Title = "Home", MenuType = MenuType.Home, Icon = "ic_home.png"});
            MenuItems.Add(new MenuItem() { Id = 1, Title = "Orario Giornaliero", MenuType = MenuType.Giornaliero, Icon = "ic_clock.png" });
            MenuItems.Add(new MenuItem() { Id = 2, Title = "Orario Completo", MenuType = MenuType.Completo, Icon = "ic_completo.png" });
            MenuItems.Add(new MenuItem() { Id = 3, Title = "Esami", MenuType = MenuType.Esami, Icon = "ic_esami.png" });
            
        }

    }
}

