using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views.ViewCells
{
    class HeaderCell : ViewCell
    {
        public HeaderCell()
        {
            var title = new Label
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                FontAttributes =  FontAttributes.Bold,
                TextColor = ColorHelper.White,
                VerticalOptions = LayoutOptions.Center
            };
            //title.SetBinding(Label.TextProperty, new Binding("Key", converter: new TitleGiorniConverter()));
            title.SetBinding(Label.TextProperty, "Key");

            View = new StackLayout
            {
                Padding = new Thickness(10, 5, 10, 5),
                BackgroundColor = ColorHelper.Blue700,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };

        }
    }
}
