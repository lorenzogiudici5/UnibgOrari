using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using Xamarin.Forms;
using OrariUnibg.Helpers;
using OrariUnibg.Services.Database;

namespace OrariUnibg.Views.ViewCells
{
    class OrarioGiornCell : ViewCell
    {
        #region Constructor
        public OrarioGiornCell()
        {
            View = getView();
        }
        #endregion
        #region Private Fields
        private Label _lblCorso;
        private Label _lblCodice;
        private Label _lblAula;
        private Label _lblOra;
        private Label _lblDocente;
        private Label _lblNote;
        #endregion

        #region Private Methods
        private View getView()
        {
            _lblCorso = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                TextColor = ColorHelper.Blue,
                //HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _lblCorso.SetBinding(Label.TextProperty, "Insegnamento");

            _lblCodice = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Micro),
                TextColor = ColorHelper.Gray,
                IsVisible = false,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };
            _lblCodice.SetBinding(Label.TextProperty, "Codice");
            
            _lblAula = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Gray,
                //HorizontalOptions = LayoutOptions.FillAndExpand
            };
            _lblAula.SetBinding(Label.TextProperty, "Aula");

            _lblOra = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Gray,
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            _lblOra.SetBinding(Label.TextProperty, "Ora");
           
            _lblDocente = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small, FontAttributes.Bold),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };
            _lblDocente.SetBinding(Label.TextProperty, "Docente");

            _lblNote = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Small),
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.StartAndExpand,
                IsVisible = false,
            };
            _lblNote.SetBinding(Label.TextProperty, "Note");

            //lblOra.SetBinding(Label.TextColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));
            
            //lblAula.SetBinding(Label.TextColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));
            
           

            var grid = new Grid()
            {
                Padding = new Thickness(10, 10, 10, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 0,
                ColumnSpacing = 5,
                RowDefinitions = 
                {
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions = 
                { 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                }
            };
            grid.SetBinding(Grid.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

            grid.Children.Add(_lblCorso, 0, 2, 0, 1);
            //grid.Children.Add(_lblCodice, 1, 2, 0, 1);
            grid.Children.Add(_lblAula, 0, 1, 1, 2);
            grid.Children.Add(_lblCodice, 1, 2, 1, 2);
            grid.Children.Add(_lblOra, 0, 1, 2, 3);
            grid.Children.Add(_lblDocente, 1, 2, 2, 3);
            grid.Children.Add(_lblNote, 0, 2, 3, 4);

            //var layout = new StackLayout()
            //{
            //    Padding = new Thickness(10, 10, 10, 10),
            //    Spacing = 0,
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    Orientation = StackOrientation.Vertical,
            //    Children = { 
            //        lblCorso, 
            //        lblAula, 
            //        new StackLayout(){HorizontalOptions = LayoutOptions.FillAndExpand, Orientation = StackOrientation.Horizontal, Children ={lblOra, lblDocente} },
            //        lblNote
            //    }
            //};

            //layout.SetBinding(StackLayout.BackgroundColorProperty, new Binding("Note", converter: new NoteBackgroundConverter()));

            MessagingCenter.Subscribe<OrarioGiornaliero, CorsoGiornaliero>(this, "item_clicked", (sender, arg) =>
            {
                if (arg.Insegnamento == _lblCorso.Text && arg.Ora == _lblOra.Text & arg.Aula == _lblAula.Text)
                {
                    if (_lblNote.Text != String.Empty)
                    {
                        if (_lblNote.IsVisible)
                            _lblNote.IsVisible = false;
                        else
                            _lblNote.IsVisible = true;
                    }

                    if (_lblCodice.IsVisible)
                        _lblCodice.IsVisible = false;
                    else
                        _lblCodice.IsVisible = true;
                }
                else
                    return;
            });

            MessagingCenter.Subscribe<TabbedDayView, Orari>(this, "orari_clicked", (sender, arg) =>
            {
                if (arg.Insegnamento == _lblCorso.Text && arg.Ora == _lblOra.Text & arg.Aula == _lblAula.Text)
                {
                    if (_lblNote.Text != String.Empty)
                    {
                        if (_lblNote.IsVisible)
                            _lblNote.IsVisible = false;
                        else
                            _lblNote.IsVisible = true;
                    }

                    if (_lblCodice.IsVisible)
                        _lblCodice.IsVisible = false;
                    else
                        _lblCodice.IsVisible = true;

                }
                else
                    return;
            });

            return grid;
        }
        #endregion
        
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
                        return Color.Transparent; // per sfondo layout
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
