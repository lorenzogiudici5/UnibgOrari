using OrariUnibg.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views.ViewCells
{
    class ListaCorsiCell : ViewCell
    {
        #region Constructor
        public ListaCorsiCell()
        {
            View = getView();
        }
        #endregion

        #region Private Fields

        #endregion

        #region Private Methods
        private View getView()
        {
            var lblCorso = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = ColorHelper.Blue,
            };

            var lblDocente = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            var lblAulaOra = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small)
            };

            lblCorso.SetBinding(Label.TextProperty, "Insegnamento");
            lblDocente.SetBinding(Label.TextProperty, "Docente");
            lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

            var layout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Children = 
                { 
                    lblCorso,
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {lblAulaOra, lblDocente}},
                }
            };
            return layout;
        }
        #endregion
    }
}
