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
		public string Icon {get;set;}
        //public string Subtitle { get; set; }
		public MenuType MenuType { get; set; }
    }

    public enum MenuType
    {
        Home,
        Giornaliero,
        Completo,
        Esami
    }
}
