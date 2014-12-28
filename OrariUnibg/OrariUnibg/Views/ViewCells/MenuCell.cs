using OrariUnibg.Helpers;
using OrariUnibg.Views;
using OrariUniBg.Models;
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
        private Image _iconDeSelected;
        private Image _iconSelected;
        public MenuItem _menu;
        #endregion
        public View getView()
        {
            _lblTitle = new Label
            {
                TextColor = ColorHelper.Black,
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
                VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _lblTitle.SetBinding(Label.TextProperty, MasterDetailViewModel.TitlePropertyName);
            _lblTitle.SetBinding(Label.TextColorProperty, new Binding("Selected", converter: new LabelColorSelected()));

            _iconDeSelected = new Image()
            {
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _iconDeSelected.SetBinding(Image.SourceProperty, "IconDeSelected");
            _iconDeSelected.SetBinding(Image.IsVisibleProperty, new Binding("Selected", converter: new IconVisibileDeSelected()));

            _iconSelected = new Image()
            {
                //VerticalOptions = LayoutOptions.CenterAndExpand
            };
            _iconSelected.SetBinding(Image.SourceProperty, "IconSelected");
            _iconSelected.SetBinding(Image.IsVisibleProperty, new Binding("Selected", converter: new IconVisibileSelected()));
            //_icon.SetBinding(Image.SourceProperty, new Binding("Selected", converter: new LabelColorSelected()));


            var layout = new StackLayout()
            {
                Orientation = StackOrientation.Horizontal,
                Padding = new Thickness(15, 7, 15, 7),
                //VerticalOptions = LayoutOptions.CenterAndExpand,
                Spacing = 25,
                Children = { _iconDeSelected, _iconSelected, _lblTitle }
            };

            //MessagingCenter.Subscribe<MasterView, Page>(this, "menuItem_clicked", (sender, arg) =>
            //{
            //    if (arg.Title == _lblTitle.Text)
            //    {
            //        layout.BackgroundColor = ColorHelper.LightBlue;
            //    }
            //});

            return layout;
        }

        //protected override void OnBindingContextChanged()
        //{
        //    base.OnBindingContextChanged();
        //    _menu = (MenuItem)BindingContext;
        //}

        //protected override void OnAppearing()
        //{
        //    _icon.SetBinding(Image.SourceProperty, "IconToShow");
        //    base.OnAppearing();

        //}
    }

    public class LabelColorSelected : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return ColorHelper.Blue;
                else
                    return ColorHelper.Black;
            }

            return ColorHelper.Black;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconVisibileSelected : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return true;
                else
                    return false;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IconVisibileDeSelected : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is bool)
            {
                if ((bool)value)
                    return false;
                else
                    return true;
            }

            return false;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
