using OrariUniBg.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrariUnibg.Models
{
    public class MenuItem : BaseModel
    {
        public MenuItem()
		{
			MenuType = MenuType.Home;
		}
        //public string Icon {get;set;}
        //public string Subtitle { get; set; }
		public MenuType MenuType { get; set; }

        public string IconSelected { get {return Icon+"_blue.png";}}
        public string IconDeSelected { get { return Icon + "_dark.png"; } }
        public bool _selected;
        public const string SelectedPropertyName = "Selected";
        public bool Selected
        {
            get { return _selected; }
            set 
            { 
                SetProperty(ref _selected, value, SelectedPropertyName); 
            }
        }
    }

    public enum MenuType
    {
        Home,
        Giornaliero,
        Completo,
        Esami
    }
}
