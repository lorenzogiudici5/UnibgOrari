using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.View.ViewCells;
using Xamarin.Forms;

namespace OrariUnibg.View
{
    public class SelectGiornaliero : ContentPage
    {
        Picker pickerFacoltà;
        Picker pickerLaurea;
        Picker pickerOrder;
        DatePicker pickData;
        ActivityIndicator activityIndicator;
        Label lblError;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        public SelectGiornaliero()
        {
            Title = "Giornaliero";
            listFacolta = Facolta.facolta;
            pickerFacoltà = new Picker()
            {
                Title = "Facoltà"
            };
            pickerLaurea = new Picker()
            {
                Title = "Corso di Laurea"
            };

            foreach (var f in listFacolta)
            {
                pickerFacoltà.Items.Add(f.Nome);
            }

            pickerFacoltà.SelectedIndexChanged += (sender, args) =>
            {
                var s = pickerFacoltà.Items[pickerFacoltà.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                dictionaryLauree = LaureeDictionary.getLauree(facolta);
                pickerLaurea.Items.Clear();

                foreach (var item in dictionaryLauree)
                    pickerLaurea.Items.Add(item.Key);

                pickerLaurea.SelectedIndex = 0;
            };

            pickData = new DatePicker()
            {
                Format = "D"
            };

            pickerOrder = new Picker()
            {
                Title = "Ordina per..",
            };
            String[] ordina = new String[] { "Alfabetico", "Orario"};

            foreach (var x in ordina)
                pickerOrder.Items.Add(x);

            pickerFacoltà.SelectedIndex = Settings.Facolta;
            System.Diagnostics.Debug.WriteLine("FACOLTA: " + Settings.Facolta);
            pickerLaurea.SelectedIndex = Settings.Laurea;
            pickerOrder.SelectedIndex = Settings.Order;

            var btn = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Ricerca",
                BackgroundColor = Color.FromHex("10528c"),
                TextColor = Color.White,
                BorderColor = Color.White,
            };

            activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Fill
            };

            lblError = new Label()
            {
                Text = "ORARIO NON DISPONIBILE O IN CORSO DI DEFINIZIONE",
                TextColor = Color.Red,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                Font = Font.SystemFontOfSize(NamedSize.Small),
                IsVisible = false,
            };
            
            btn.Clicked += btn_Clicked;
            var layout = new StackLayout()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(10, 10, 10, 10),
                Orientation = StackOrientation.Vertical,
                Children = { pickerFacoltà, pickerLaurea, pickData, pickerOrder, lblError, activityIndicator, btn, }
            };

            Content = layout;
        }

        private async void btn_Clicked(object sender, EventArgs e)
        {
            lblError.IsVisible = false;
            activityIndicator.IsVisible = true;

            Facolta fac = listFacolta.Where(x => x.Nome == pickerFacoltà.Items[pickerFacoltà.SelectedIndex]).First();
            int facolta = fac.IdFacolta;
            string db = fac.DB;
            int laureaId = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Value;
            string laurea = dictionaryLauree.Where(x => x.Key == pickerLaurea.Items[pickerLaurea.SelectedIndex]).First().Key;
            string data = pickData.Date.ToString("dd'/'MM'/'yyyy");
            int order = pickerOrder.SelectedIndex;

            string s = await Web.GetOrarioGiornaliero(db, facolta, laureaId, data);

            Settings.Facolta = pickerFacoltà.SelectedIndex;
            Settings.Laurea = pickerLaurea.SelectedIndex;
            Settings.Order = order;

            if (s == string.Empty)
            {
                activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                return;
            }

            List<CorsoGiornaliero> lista = Web.GetSingleOrarioGiornaliero(s, order);
            activityIndicator.IsVisible = false;
            if (lista.Count > 0)
                await this.Navigation.PushAsync(new OrarioGiornaliero(lista, fac.Nome, laurea, data));
            else
            {
                lblError.IsVisible = true;
                return;
            }
                
        }
    }
}
