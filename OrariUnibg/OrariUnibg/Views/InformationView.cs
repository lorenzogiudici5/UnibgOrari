using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class InformationView : ContentPage
    {
        #region Constructor
        public InformationView()
        {
            Title = "Informazioni";
            BackgroundColor = ColorHelper.White;
            Content = getView();
        }      
        #endregion

        #region Private Fields
        private Entry _entryNome;
        private Entry _entryCognome;
        private Entry _entryMail;
        private Label _lblMail;
        private Picker _pickFacolta;
        private Picker _pickLaurea;
        private Picker _pickAnno;
        private Label _lblNotific;
        private Switch _switchNotific;
        private Label _lblSync;
        private Switch _switchSync;
        private int limit = 3;
        private ActivityIndicator _activityIndicator;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        #endregion

        #region Private Methods
        private View getView()
        {
            listFacolta = Facolta.facolta;

            _entryNome = new Entry()
            {
                Placeholder = "Nome",
                HorizontalOptions = LayoutOptions.FillAndExpand,
            };
            _entryCognome = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "Cognome"
            };
            _entryMail = new Entry()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Placeholder = "n.cognome2"
            };
            _lblMail = new Label()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Font = Font.SystemFontOfSize(NamedSize.Medium),
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "@studenti.unibg.it"
            };

            _pickFacolta = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Facoltà"
            };
            _pickLaurea = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Laurea",
                IsEnabled = false,
            };
            _pickAnno = new Picker()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Title = "Anno",
                IsEnabled = false
            };

            foreach (var f in listFacolta)
            {
                _pickFacolta.Items.Add(f.Nome);
            }

            _pickFacolta.SelectedIndexChanged += (sender, args) =>
            {
                var s = _pickFacolta.Items[_pickFacolta.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                _pickLaurea.Items.Clear();

                foreach (var item in dictionaryLauree)
                    _pickLaurea.Items.Add(item.Key);

                _pickLaurea.SelectedIndex = 0;
                _pickLaurea.IsEnabled = true;
            };

            String[] anni = new String[] { "1° Anno", "2° Anno", "3° Anno", "4° Anno", "5° Anno" };
            _pickLaurea.SelectedIndexChanged += (sender, args) =>
            {
                int i = 1;
                _pickAnno.IsEnabled = true;

                if (_pickLaurea.Items.Count > 0)
                {
                    if (_pickLaurea.Items[_pickLaurea.SelectedIndex].Contains("Magistrale"))
                        limit = 5;
                    else if (_pickLaurea.Items[_pickLaurea.SelectedIndex].Contains("LM"))
                        limit = 2;
                    else limit = 3;

                    _pickAnno.Items.Clear();
                    foreach (var x in anni)
                    {
                        if (i > limit)
                            break;
                        _pickAnno.Items.Add(x);
                        i++;
                    }
                    _pickAnno.SelectedIndex = 0;
                }
                
            };

            _lblNotific = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Notifiche variazioni lezioni"
            };
            _switchSync = new Switch() { IsToggled = true};
            _lblSync = new Label()
            {
                HorizontalOptions = LayoutOptions.StartAndExpand,
                Text = "Sincronizzazione bakgruound"
            };
            _switchNotific = new Switch() { IsToggled = true };

            _activityIndicator = new ActivityIndicator()
            {
                HorizontalOptions = LayoutOptions.FillAndExpand,
                IsRunning = true,
                IsVisible = false,
            };

            var grid = new Grid()
            {
                Padding = new Thickness(15, 10, 15, 10),
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.CenterAndExpand,
                RowSpacing = 8,
                ColumnSpacing = 5,
                RowDefinitions = 
                {
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                     new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions = 
                { 
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(0.6, GridUnitType.Star) },
                }
            };
           
            grid.Children.Add(_entryNome, 0, 2, 0, 1);
            grid.Children.Add(_entryCognome, 0, 2, 1, 2);
            grid.Children.Add(_entryMail, 0, 1, 2, 3);
            grid.Children.Add(_lblMail, 1, 2, 2, 3);
            grid.Children.Add(_pickFacolta, 0, 2, 3, 4);
            grid.Children.Add(_pickLaurea, 0, 1, 4, 5);
            grid.Children.Add(_pickAnno, 1, 2, 4, 5);
            grid.Children.Add(_lblSync, 0, 1, 5, 6);
            grid.Children.Add(_switchSync, 1, 2, 5, 6);
            grid.Children.Add(_lblNotific, 0, 1, 6, 7);
            grid.Children.Add(_switchNotific, 1, 2, 6, 7);
            grid.Children.Add(_activityIndicator, 0, 2, 7, 8);

            ToolbarItem tbiNext = new ToolbarItem("Avanti", "ic_next.png", async () =>
            {
                _activityIndicator.IsVisible = true;
                Facolta fac = listFacolta.Where(x => x.Nome == _pickFacolta.Items[_pickFacolta.SelectedIndex]).First();
                int facolta = fac.IdFacolta;
                string db = fac.DB;
                int laurea = dictionaryLauree.Where(x => x.Key == _pickLaurea.Items[_pickLaurea.SelectedIndex]).First().Value;
                int anno = _pickAnno.SelectedIndex + 1;
                Settings.Nome = _entryNome.Text;
                Settings.Cognome = _entryCognome.Text;
                Settings.Email = _entryMail.Text + _lblMail.Text;
                Settings.Facolta = facolta;
                Settings.DBfacolta = db;
                Settings.Laurea = laurea;
                Settings.Anno = anno;
                Settings.BackgroundSync = _switchSync.IsToggled;
                Settings.Notify = _switchNotific.IsToggled;

                Settings.LaureaIndex = _pickLaurea.SelectedIndex;
                Settings.FacoltaIndex = _pickFacolta.SelectedIndex;
                Settings.AnnoIndex = anno;

                string completo = await Web.GetOrarioCompleto("completo", db, facolta, laurea, anno);
                string secondo = await Web.GetOrarioCompleto("secondo", db, facolta, laurea, anno);

                List<CorsoCompleto> lista_completo = Web.GetSingleOrarioCompleto(completo);
                List<CorsoCompleto> lista_secondo = Web.GetSingleOrarioCompleto(secondo);

                lista_completo.AddRange(lista_secondo);
                _activityIndicator.IsVisible = false;

                Settings.PrimoAvvio = false;
                await Navigation.PushAsync(new ListaCorsi(lista_completo));
                    //Navigation.PopModalAsync();
            }, 0, 0); 
            //if (Device.OS == TargetPlatform.Android)
            //{ // BUG: Android doesn't support the icon being null
            //    tbiNext = new ToolbarItem("Avanti", "ic_menu.png", () =>
            //    {
            //        Navigation.PushAsync(new MasterDetailView());
            //    }, 0, 0);
            //}
            //if (Device.OS == TargetPlatform.WinPhone)
            //{
            //    tbi = new ToolbarItem("Avanti", "ic_menu.png", () =>
            //    {
            //        Navigation.PushAsync(new MasterDetailView());
            //    }, 0, 0);
            //}
            ToolbarItems.Add(tbiNext);
            return grid;
        }
        #endregion
    }
}
