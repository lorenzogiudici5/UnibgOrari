using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Helpers;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioComplCell : ViewCell
    {
        public OrarioComplCell()
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

            var lblInizioFine = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Italic),
                HorizontalOptions = LayoutOptions.Start,
                IsVisible = false,
            };

            var Lunedi = new Label() { Text = "LUNEDI:", Font =Font.SystemFontOfSize(NamedSize.Small)};
            var Martedi = new Label() { Text = "MARTEDI:", Font =Font.SystemFontOfSize(NamedSize.Small)};
            var Mercoledi = new Label() { Text = "MERCOLEDI:", Font =Font.SystemFontOfSize(NamedSize.Small)};
            var Giovedi = new Label() { Text = "GIOVEDI:", Font =Font.SystemFontOfSize(NamedSize.Small)};
            var Venerdi = new Label() { Text = "VENERDI:", Font =Font.SystemFontOfSize(NamedSize.Small)};
            var Sabato = new Label() { Text = "SABATO:", Font =Font.SystemFontOfSize(NamedSize.Small)};

            var lblLunedi = new Label()
            {
                Font =Font.SystemFontOfSize(NamedSize.Small)
            };
            var lblMartedi = new Label();
            lblMartedi.Font =Font.SystemFontOfSize(NamedSize.Small);
            var lblMercoledi = new Label();
            lblMercoledi.Font =Font.SystemFontOfSize(NamedSize.Small);
            var lblGiovedi = new Label();
            lblGiovedi.Font =Font.SystemFontOfSize(NamedSize.Small);
            var lblVenerdi = new Label();
            lblVenerdi.Font =Font.SystemFontOfSize(NamedSize.Small);
            var lblSabato = new Label();
            lblSabato.Font = Font.SystemFontOfSize(NamedSize.Small);

            lblCorso.SetBinding(Label.TextProperty, "Insegnamento");
            lblDocente.SetBinding(Label.TextProperty, "Docente");
            lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");
            
            lblLunedi.SetBinding(Label.TextProperty, "Lezioni[0].AulaOra");
            lblLunedi.SetBinding(Label.IsVisibleProperty, "Lezioni[0].isVisible");
            Lunedi.SetBinding(Label.IsVisibleProperty, "Lezioni[0].isVisible");

            lblMartedi.SetBinding(Label.TextProperty, "Lezioni[1].AulaOra");
            lblMartedi.SetBinding(Label.IsVisibleProperty, "Lezioni[1].isVisible");
            Martedi.SetBinding(Label.IsVisibleProperty, "Lezioni[1].isVisible");

            lblMercoledi.SetBinding(Label.TextProperty, "Lezioni[2].AulaOra");
            lblMercoledi.SetBinding(Label.IsVisibleProperty, "Lezioni[2].isVisible");
            Mercoledi.SetBinding(Label.IsVisibleProperty, "Lezioni[2].isVisible");

            lblGiovedi.SetBinding(Label.TextProperty, "Lezioni[3].AulaOra");
            lblGiovedi.SetBinding(Label.IsVisibleProperty, "Lezioni[3].isVisible");
            Giovedi.SetBinding(Label.IsVisibleProperty, "Lezioni[3].isVisible");

            lblVenerdi.SetBinding(Label.TextProperty, "Lezioni[4].AulaOra");
            lblVenerdi.SetBinding(Label.IsVisibleProperty, "Lezioni[4].isVisible");
            Venerdi.SetBinding(Label.IsVisibleProperty, "Lezioni[4].isVisible");

            lblSabato.SetBinding(Label.TextProperty, "Lezioni[5].AulaOra");
            lblSabato.SetBinding(Label.IsVisibleProperty, "Lezioni[5].isVisible");
            Sabato.SetBinding(Label.IsVisibleProperty, "Lezioni[5].isVisible");

            var label = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children = { Lunedi, Martedi, Mercoledi, Giovedi, Venerdi, Sabato }
            };
            var lblLezioni = new StackLayout()
            {
                Orientation = StackOrientation.Vertical,
                Spacing = 0,
                Children = { lblLunedi, lblMartedi, lblMercoledi, lblGiovedi, lblVenerdi, lblSabato}
            };

            var layout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Children = 
                { 
                    lblCorso,
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {label, lblLezioni}},
                    new StackLayout() {Orientation = StackOrientation.Horizontal, Children = {lblInizioFine, lblDocente}},
                }
            };
            View = layout;
        }
    }
}
