using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;

namespace OrariUnibg.View.ViewCells
{
    class OrarioGiornCell : ViewCell
    {
        public OrarioGiornCell()
        {
            var lblCorso = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = Color.Blue,
                //HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var lblOra = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Gray,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };

            var lblAula = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Gray,
                //HorizontalOptions = LayoutOptions.FillAndExpand
            };

            var lblDocente = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            var lblNote = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                IsVisible = false,
            };

            lblCorso.SetBinding(Label.TextProperty, "Insegnamento");
            lblOra.SetBinding(Label.TextProperty, "Ora");
            lblAula.SetBinding(Label.TextProperty, "Aula");
            lblDocente.SetBinding(Label.TextProperty, "Docente");
            lblNote.SetBinding(Label.TextProperty, "Note");

            var layout = new StackLayout()
            {
                Padding = new Thickness(10, 10, 10, 10),
                Spacing = 0,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Orientation = StackOrientation.Vertical,
                Children = { 
                    lblCorso, 
                    lblAula, 
                    new StackLayout(){HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, Children ={lblOra, lblDocente} },
                    lblNote
                }
            };

            layout.SetBinding(StackLayout.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

            MessagingCenter.Subscribe<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", (sender, arg) =>
            {
                if (arg.Insegnamento == lblCorso.Text && arg.Ora == lblOra.Text && lblNote.Text != String.Empty)
                {
                    if (lblNote.IsVisible)
                        lblNote.IsVisible = false;
                    else
                        lblNote.IsVisible = true;
                }
                else
                    return;
            });

            View = layout;
        }
    }

    public class NoteBackgroundConverter : IValueConverter
    {

        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string)
            {
                switch ((string)value)
                {
                    case "Sospensione lezione":
                        return Color.FromHex("FF6666");
                    case "Cambio aula":
                        return Color.FromHex("FFFF66");
                    case "Attività accademica":
                        return Color.FromHex("B0B0FF");
                    case "Attività integrativa":
                        return Color.Pink;
                    case "Esame":
                        return Color.FromHex("A0FFA0");
                    case "Alta formazione":
                        return Color.FromHex("A0FFFF");
                    case "Recupero lezione":
                        return Color.FromHex("00DD00");
                    default:
                        return Color.Transparent;
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
