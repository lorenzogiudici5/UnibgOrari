using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows.Input;
using System.Threading.Tasks;
using Xamarin.Forms;
using OrariUniBg.Models;
using OrariUnibg.Models;
using OrariUnibg.Helpers;

namespace OrariUniBg.ViewModels
{
    public class MasterDetailViewModel : BaseViewModel
    {
        public ObservableCollection<OrariUnibg.Models.MenuItem> MenuItems { get; set; }
        public MasterDetailViewModel()
        {
            Title = "Menu";
            Subtitle = "Menu";
            MenuItems = new ObservableCollection<OrariUnibg.Models.MenuItem>();

			MenuItems.Add(new OrariUnibg.Models.MenuItem() { Id = 0, Title = "Home", MenuType = MenuType.Home, Icon = "ic_home"});
			MenuItems.Add(new OrariUnibg.Models.MenuItem() { Id = 1, Title = "Orario Giornaliero", MenuType = MenuType.Giornaliero, Icon = "ic_clock" });
            MenuItems.Add(new OrariUnibg.Models.MenuItem() { Id = 2, Title = "Orario Completo", MenuType = MenuType.Completo, Icon = "ic_completo" });

			if (Settings.SuccessLogin) {
				MenuItems.Add(new OrariUnibg.Models.MenuItem() { Id = 3, Title = "Impostazioni", MenuType = MenuType.Impostazioni, Icon = "ic_settings" });
			}

//            MenuItems.Add(new MenuItem() { Id = 3, Title = "Esami", MenuType = MenuType.Esami, Icon = "ic_esami.png" });
        }

    }
}

