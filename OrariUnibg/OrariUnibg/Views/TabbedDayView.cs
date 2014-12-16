using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services.Database;
using OrariUnibg.Views.ViewCells;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    public class TabbedDayView : ContentPage
    {
        #region Constructor
        public TabbedDayView()
        {
            this.SetBinding(ContentPage.TitleProperty, "Day");
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView _listView;
        private Label _lblDay;
        private Label _lblDate;
        private Label _lblInfo;
        private ActivityIndicator _activityIndicator;
        #endregion

        #region Private Methods
        private View getView()
        {
            _lblDay = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Large, FontAttributes.Bold),
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };
            _lblDay.SetBinding(Label.TextProperty, "Day");

            _lblDate = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Large, FontAttributes.Bold),
                HorizontalOptions = LayoutOptions.StartAndExpand,
            };
            _lblDate.SetBinding(Label.TextProperty, "DateString");

            _lblInfo = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = "Rilassati! Non hai lezioni!",
                TextColor = ColorHelper.Green,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };
            _lblInfo.SetBinding(Label.IsVisibleProperty, new Binding("ListaLezioni", converter: new IsVisibleCountConverter()));

            _listView = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioGiornCell)),
                HasUnevenRows = true,
            };
            _listView.SetBinding(ListView.ItemsSourceProperty, "ListaLezioni");
            _listView.ItemSelected += _listView_ItemSelected;

            _activityIndicator = new ActivityIndicator()
            {
                IsRunning = false,
                IsVisible = false,
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            var layout = new StackLayout()
            {
                Padding = new Thickness(10),
                Spacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = 
                {
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Spacing = 5, Children = {_lblDay, _lblDate}},
                    _lblInfo,
                    _listView,
                    _activityIndicator
                }
            };

            MessagingCenter.Subscribe<TabbedHomeView, bool>(this, "sync", (sender, arg) => 
            {
                if(arg)
                {
                    _activityIndicator.IsRunning = true;
                    _activityIndicator.IsVisible = true;
                }
                else
                {
                    _activityIndicator.IsRunning = false;
                    _activityIndicator.IsVisible = false;
                }

            });

            return layout;
        }
        #endregion

        #region Handlers
        void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var x = (Orari)_listView.SelectedItem;

            MessagingCenter.Send<TabbedDayView, Orari>(this, "orari_clicked", x);

            ((ListView)sender).SelectedItem = null;
        }
        #endregion
    }

    public class IsVisibleCountConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Orari>)
            {
                var x = (IEnumerable<Orari>)value;
                switch (x.Count())
                {
                    case 0:
                        return true;
                    default:
                        return false;
                }
            }

            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
