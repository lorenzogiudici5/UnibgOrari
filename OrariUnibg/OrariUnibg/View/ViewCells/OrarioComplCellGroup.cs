using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;

namespace OrariUnibg.View.ViewCells
{
    class OrarioComplCellGroup : ViewCell
    {
        public OrarioComplCellGroup()
        {
            var lblCorso = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = Color.Blue,
            };

            var lblDocente = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            var lblAulaOra = new Label()
            {
                Font =Font.SystemFontOfSize(NamedSize.Small)
            };

            lblCorso.SetBinding(Label.TextProperty, "Insegnamento");
            lblDocente.SetBinding(Label.TextProperty, "Docente");
            //lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");

            lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

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


            var layout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Children = 
                { 
                    lblCorso,
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {lblAulaOra, lblDocente}},
                }
            };
            View = layout;
        }
    }
}
