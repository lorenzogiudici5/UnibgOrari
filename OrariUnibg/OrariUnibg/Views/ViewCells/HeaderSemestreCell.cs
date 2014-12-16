using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views.ViewCells
{
    class HeaderSemestreCell : ViewCell
    {
        #region Constructor
        public HeaderSemestreCell()
        {
            View = getView();
        }

        #endregion
        private Label _lblTitle;
        #region Private Fields

        #endregion

        #region Private Methods
        private View getView()
        {
            _lblTitle = new Label
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium, FontAttributes.Bold),
                TextColor = Color.Black,
                VerticalOptions = LayoutOptions.Center
            };
            _lblTitle.SetBinding(Label.TextProperty, new Binding("Key", converter: new TitleGiorniConverter()));

            var view = new StackLayout
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10),
                Children = { _lblTitle }
            };

            return view;
        }
        #endregion
    }

    public class TitleGiorniConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int)
            {
                switch((int)value)
                {
                    case 1:
                        return "PRIMO SEMESTRE";
                    case 2:
                        return "SECONDO SEMESTRE";
                }
            }
            return String.Empty;

        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
