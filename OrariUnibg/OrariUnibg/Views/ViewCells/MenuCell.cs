using OrariUnibg.Helpers;
using OrariUnibg.Views;
using OrariUniBg.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUniBg.Views.ViewCells
{
    public class MenuCell : ViewCell
    {
        #region Constructor
        public MenuCell()
        {
            View = getView();
        }
        #endregion
        #region Private Fields
        private Label _lblTitle;
        private Image _image;
        #endregion
        public View getView()
        {
            _lblTitle = new Label
            {
                TextColor = ColorHelper.White,
                Font = Font.SystemFontOfSize(NamedSize.Large),
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _lblTitle.SetBinding(Label.TextProperty, MasterDetailViewModel.TitlePropertyName);

            _image = new Image()
            {
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _image.SetBinding(Image.SourceProperty, MasterDetailViewModel.IconPropertyName);

            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(15, 10, 15, 10),
                //VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 15,
                Children = { _image, _lblTitle }
            };

            //MessagingCenter.Subscribe<MasterView, Page>(this, "menuItem_clicked", (sender, arg) =>
            //{
            //    if(arg.Title == _lblTitle.Text)
            //    {
            //        layout.BackgroundColor = ColorHelper.LightBlue;
            //    }
            //});

            return layout;
        }
    }
}
