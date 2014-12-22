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
                Font = Font.SystemFontOfSize(NamedSize.Small),
            };

            _lblAulaOra = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
            };
            _lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 5, 15, 0),
                Spacing = 5,
                Orientation = StackOrientation.Horizontal,
                Children = { _lblTitleUtenza, _lblAulaOra}
            };

            return layout;
        }
        #endregion

    }
}
