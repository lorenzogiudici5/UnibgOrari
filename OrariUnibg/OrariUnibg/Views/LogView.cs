using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    public class LogView : ContentPage
    {
        #region Constructor
        public LogView()
        {
            Title = "Log";
            _db = new DbSQLite();
            Content = getView();
        }
        #endregion

        #region Private Fields
        private ListView _listView;
        private DbSQLite _db;
        private ToolbarItem tbiDelete;
        #endregion

        #region Private Methods
        private View getView()
        {
            _listView = new ListView()
            {
                HasUnevenRows = true,
                ItemsSource = _db.GetAllLogs(),
                ItemTemplate = new DataTemplate(() =>
                {
                    Label date = new Label() { TextColor = ColorHelper.Gray, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                    date.SetBinding(Label.TextProperty, "Date");

                    Label log = new Label() { TextColor = ColorHelper.Black, FontSize = Device.GetNamedSize(NamedSize.Small, this) };
                    log.SetBinding(Label.TextProperty, "Log");

                    return new ViewCell
                    {
                        View = new StackLayout
                        {
                            Orientation = StackOrientation.Horizontal,
                            Spacing = 10,
                            Padding = new Thickness(10, 10, 10, 10),
                            Children = { date, log }
                        }
                    };
                })
            };
            //_listView.SetBinding(Label.TextProperty, new Binding("Log"));

            var layout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,

                Children = { _listView }
            };


            //Toolbar Item
            tbiDelete = new ToolbarItem("Rimuovi tutti", null, deleteAll, 0, 0);
            ToolbarItems.Add(tbiDelete);

            return layout;
        }
        #endregion

        #region Event Handlers
        private async void deleteAll()
        {
            //cancella tutti i dati di log
            _db.ClearLog();
            await this.Navigation.PopAsync();
        }
        #endregion

    }
}
