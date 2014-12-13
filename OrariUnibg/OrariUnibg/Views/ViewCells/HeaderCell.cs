using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;

namespace OrariUnibg.Views.ViewCells
{
    class HeaderCell : ViewCell
    {
        public HeaderCell()
        {
            var title = new Label
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium, FontAttributes.Bold),
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Center
            };
            //title.SetBinding(Label.TextProperty, new Binding("Key", converter: new TitleGiorniConverter()));
            title.SetBinding(Label.TextProperty, "Key");

            View = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { title }
            };

        }
    }
}
