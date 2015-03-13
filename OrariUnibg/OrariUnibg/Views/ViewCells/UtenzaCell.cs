using OrariUnibg.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views.ViewCells
{
    class UtenzaCell : ViewCell
    {
        #region Constructro
        public UtenzaCell()
        {
            View = getView();
        }
        #endregion

        #region Private Fields
        private Label _lblTitleUtenza;
        private Label _lblAulaOra;
        #endregion

        #region Private Methods
        private View getView()
        {
            _lblTitleUtenza = new Label()
            {
                Text = "USO UTENZA:",
                FontSize = Device.GetNamedSize(NamedSize.Small, this)
            };

            _lblAulaOra = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this)
            };
            _lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

            var layout = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
                Padding = new Thickness(10, 10, 10, 10),
                Spacing = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { _lblTitleUtenza, _lblAulaOra}
            };

            return layout;
        }
        #endregion

    }
}
