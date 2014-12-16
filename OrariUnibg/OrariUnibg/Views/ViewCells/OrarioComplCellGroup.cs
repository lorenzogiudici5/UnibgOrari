using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Services.Database;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioComplCellGroup : ViewCell
    {
        #region Constructor
        public OrarioComplCellGroup()
        {
            View = getView();
        }
        #endregion

        #region Private Fields
        private StackLayout _layout;
        private Label _lblInsegnamento;
        private Label _lblDocente;
        private Label _lblAulaOra;
        #endregion

        #region Private Methods

        #endregion
        private View getView()
        {
            _lblInsegnamento = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = ColorHelper.Blue,
            };

            _lblDocente = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            _lblAulaOra = new Label()
            {
                Font =Font.SystemFontOfSize(NamedSize.Small)
            };

            _lblInsegnamento.SetBinding(Label.TextProperty, "Insegnamento");
            _lblDocente.SetBinding(Label.TextProperty, "Docente");
            //lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");

            _lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

            //MessagingCenter.Subscribe<OrarioCompleto, CorsoCompleto>(this, "completo_clicked", (sender, arg) =>
            //{
            //    if (arg.Insegnamento == lblCorso.Text)
            //    {
            //        if (lblInizioFine.IsVisible)
            //            lblInizioFine.IsVisible = false;
            //        else
            //            lblInizioFine.IsVisible = true;
            //    }
            //    else
            //        return;
            //});

            MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "select_fav", (sender, arg) =>
            {
                if(arg.Insegnamento == _lblInsegnamento.Text)
                    _layout.BackgroundColor = ColorHelper.LightBlue;
            });
            MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "deselect_fav", (sender, arg) =>
            {
                if (arg.Insegnamento == _lblInsegnamento.Text)
                    _layout.BackgroundColor = Color.Default;
            });

            _layout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Children = 
                { 
                    _lblInsegnamento,
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {_lblAulaOra, _lblDocente}},
                }
            };
            return _layout;
        }
    }
}
