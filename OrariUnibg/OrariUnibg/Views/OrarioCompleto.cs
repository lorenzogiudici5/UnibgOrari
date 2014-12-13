using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Models;
using OrariUnibg.Views.ViewCells;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class OrarioCompleto : ContentPage
    {
        ListView lv;
        public OrarioCompleto(List<CorsoCompleto> list, string facolta, string laurea, string anno, string semestre)
        {
            Title = facolta;
            var lblOrario = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = "ORARIO COMPLETO:",
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var lblLaurea = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = laurea.ToUpper(),
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            var lblAnno = new Label()
            {
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                Text = anno + ": " + semestre + " semestre",
                TextColor = Color.Navy,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };
            
            lv = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioComplCell)),
                HasUnevenRows = true
            };
            lv.ItemSelected += lv_ItemSelected;

            var l = new StackLayout() { 
                Padding = new Thickness(5, 5, 5, 5), 
                Spacing = 0,
                Children = { lblOrario, lblLaurea, lblAnno } };
            Content = new StackLayout()
            {
                Padding = new Thickness(5, 5, 5, 5),
                Orientation = StackOrientation.Vertical,
                Children = { l, lv}
            };
            //Grid grid = new Grid()
            //{
            //    VerticalOptions = LayoutOptions.FillAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //    ColumnDefinitions = 
            //    { 
            //        new ColumnDefinition { Width = new GridLength(3, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //        new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
            //    }
            //};

            //for (int i = 0; i < list.Count; i++)
            //{
            //    grid.RowDefinitions.Add(new RowDefinition { Height = GridLength.Auto });
            //    grid.Children.Add(new Label() { Text = list[i].Insegnamento }, 0, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[0].AulaOra }, 1, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[1].AulaOra }, 2, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[2].AulaOra }, 3, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[3].AulaOra }, 4, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[4].AulaOra }, 5, i);
            //    grid.Children.Add(new Label() { Font = Font.SystemFontOfSize(NamedSize.Micro), Text = list[i].Lezioni[5].AulaOra }, 6, i);
            //};

            //Content = grid;
        }

        void lv_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var x = (CorsoCompleto)lv.SelectedItem;

            MessagingCenter.Send<OrarioCompleto, CorsoCompleto>(this, "completo_clicked", x);

            ((ListView)sender).SelectedItem = null;
        }
        
    }
}
