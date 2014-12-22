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
            _db = new DbSQLite();
            this.SetBinding(ContentPage.TitleProperty, "Day");
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView _listView;
        private Label _lblDay;
        private Label _lblDate;
        private Label _lblInfo;
        private Label _lblTitleUtenza;
        private Label _lblUtenza;
        private ActivityIndicator _activityIndicator;
        private DbSQLite _db;
        private ListView _listUtenze;
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

            _listUtenze = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(UtenzaCell)),
                HasUnevenRows = true,
                VerticalOptions = LayoutOptions.EndAndExpand,
                IsEnabled = false,
            };
            _listUtenze.SetBinding(ListView.ItemsSourceProperty, "ListUtenza");
            _listUtenze.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleListUtenze()));

            _lblTitleUtenza = new Label()
            {
                Text = "USO UTENZA",
                Font = Font.SystemFontOfSize(NamedSize.Small),
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            _lblTitleUtenza.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleUsoUtenza()));

            _lblUtenza = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                VerticalOptions = LayoutOptions.EndAndExpand
            };
            _lblUtenza.SetBinding(Label.TextProperty, "AulaOra");
            _lblTitleUtenza.SetBinding(Label.IsVisibleProperty, new Binding("ListUtenza", converter: new IsVisibleUsoUtenza()));


            _activityIndicator = new ActivityIndicator()
            {
                IsRunning = false,
                IsVisible = false,
                VerticalOptions = LayoutOptions.EndAndExpand,
            };

            var layout = new StackLayout()
            {
                Padding = new Thickness(0, 10, 0, 10),
                Spacing = 10,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Children = 
                {
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Spacing = 5, Children = {_lblDay, _lblDate}},
                    _lblInfo,
                    _listView,
                    _activityIndicator,
                    new StackLayout() { Padding = new Thickness(15, 5, 15, 0), Orientation = StackOrientation.Horizontal, Spacing = 5, Children = {_lblTitleUtenza, _lblUtenza}},
                    _listUtenze,
                }
            };

            MessagingCenter.Subscribe<TabbedHomeView, bool>(this, "sync", sync); 

            return layout;
        }

        async void _listView_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var orario = (Orari)_listView.SelectedItem;

            string action;
            //if(_db.CheckAppartieneMieiCorsi(orario))
            action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Dettagli", "Rimuovi dai preferiti");

            //else
            //    action = await DisplayActionSheet(orario.Insegnamento, "Annulla", null, "Dettagli", "Aggiungi ai preferiti");

            switch (action)
            {
                case "Rimuovi dai preferiti":
                    var corso = _db.GetAllMieiCorsi().FirstOrDefault(x => x.Insegnamento == orario.Insegnamento);
                    _db.DeleteMieiCorsi(corso);
                    MessagingCenter.Send<TabbedDayView>(this, "delete_corso");
                    break;
                case "Aggiungi ai preferiti":
                    _db.Insert(new MieiCorsi() { Codice = orario.Codice, Docente = orario.Docente, Insegnamento = orario.Insegnamento });
                    break;
                default:
                case "Dettagli":
                    var nav = new DettagliCorsoView();
                    //nav.BindingContext = 
                    await this.Navigation.PushAsync(nav);
                    break;
            }

            //MessagingCenter.Send<TabbedDayView, Orari>(this, "orari_clicked", orario);

            ((ListView)sender).SelectedItem = null;
        }


        #endregion

        #region Event Handlers
        private void sync(TabbedHomeView arg1, bool arg2)
        {
            if (arg2)
            {
                _activityIndicator.IsRunning = true;
                _activityIndicator.IsVisible = true;
            }
            else
            {
                _activityIndicator.IsRunning = false;
                _activityIndicator.IsVisible = false;
            }
        }

        #endregion
    }

    #region Converter
    public class IsVisibleListUtenze : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Utenza>)
            {
                var x = (IEnumerable<Utenza>)value;
                if (x.Count() > 1)
                    return true;
                else return false;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class IsVisibleUsoUtenza : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is IEnumerable<Utenza>)
            {
                var x = (IEnumerable<Utenza>)value;
                if (x.Count() > 1 || x.Count() == 0)
                    return false;
                else return true;
            }

            return true;
        }

        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            throw new NotImplementedException();
        }
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
    #endregion 
}
