using OrariUnibg.Helpers;
using OrariUnibg.Models;
using OrariUnibg.Services;
using OrariUnibg.ViewModels;
using OrariUnibg.Views.ViewCells;
using Plugin.Connectivity;
using Plugin.Toasts;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    public class SuggerisciCorsiView : CarouselPage
    {
        public SuggerisciCorsiView()
        {
            Padding = new Thickness(0, 0, 0, 0);
            BackgroundColor = ColorHelper.Blue700;
            Title = "Corsi suggeriti";
            _listSource = new List<CorsoCompleto>();
            _viewModel = new OrariCompletoViewModel();
            getPages();
        }

        #region Private Fields
        int _index = 0;
        ContentPage page0_help;
        StackLayout layout0_help;
        ContentPage page1_settings;
        StackLayout layout1_settings;
        ContentPage page2_obbligatori;
        StackLayout layout2_obbligatori;
        ContentPage page3_scelta;
        StackLayout layout3_scelta;
        ContentPage page4_risultato;
        AbsoluteLayout layout4_risultato;
        ContentPage page5_risultatoGiorni;
        AbsoluteLayout layout5_risultatoGiorni;
        private int appearingListItemIndex = 0;
        private OrariCompletoViewModel _viewModel;
        private bool resetView = true;

        private List<CorsoCompleto> _listSource;
        private List<CorsoCompleto> _listObbligatori;
        private ListView _listViewObbligatori;
        private List<CorsoCompletoAlgoritmo> _listScelta;
        private ListView _listViewScelta;
        List<CorsoCompleto> _listSuggeriti;
        IEnumerable<IGrouping<Lezione.Day, CorsoCompletoGroupViewModel>> listaGroup;
        IEnumerable<IGrouping<int, CorsoCompleto>> _listaGroup;
        private string _listStringGroup;

        private int PESO_SOVRAPPOSIZIONE;
        private int PESO_GIORNO;
        private int NUM_CORSI;

        //private OrariCompletoViewModel _completoViewModel;

        #region Layout1
        private int _facoltàIndex = 0;
        private int _laureaIndex = 0;
        Picker _pickerFacoltà;
        Picker _pickerLaurea;
        Picker _pickerAnno;
        Picker _pickerSemestre;
        Picker _pickerNumCorsi;
        Picker _pickerPesi;
        ActivityIndicator activityIndicator;
        Label lblError;
        private int limit = 3;
        private String[] anni;
        List<Facolta> listFacolta = new List<Facolta>();
        Dictionary<string, int> dictionaryLauree = new Dictionary<string, int>();
        Dictionary<string, string> sem;
        Facolta _fac;
        string _laurea;
        string _annoString;
        string _semestreString;
        #endregion

        #region Layout0
        Switch _switchHelpHide;
        #endregion

        #region Layout4
        private ListView _lvCorsiSUggerit;
        //private List<CorsoCompleto> lista;
        private FloatingActionButtonView fab4; //TODO FAB per condivisione
        #endregion

        #region Layout5
        private ListView _lvGiorni;
        //private List<CorsoCompleto> lista;
        private FloatingActionButtonView fab5; //TODO FAB per condivisione
        #endregion
        #endregion

        #region Private Methods
        private void getPages()
        {
            //HELP
            layout0_help = initLayout0_help();
            page0_help = new ContentPage { Content = layout0_help, Title = PagesSuggerisci.Help.ToString() };

            //SETTINGS
            layout1_settings = initLayout1_settings();
            page1_settings = new ContentPage { Content = layout1_settings, Title = PagesSuggerisci.Settings.ToString() };

            //OBBLIGATORI
            layout2_obbligatori = initLayout2_obbligatori();
            page2_obbligatori = new ContentPage { Content = layout2_obbligatori, Title = PagesSuggerisci.Obbligatori.ToString() };

            //SCELTA
            layout3_scelta = initLayout3_scelta();
            page3_scelta = new ContentPage { Content = layout3_scelta, Title = PagesSuggerisci.Scelta.ToString() };

            //RISULTATO ELENCO CORSI
            layout4_risultato = initLayout4_risultato();
            page4_risultato = new ContentPage { Content = layout4_risultato, Title = PagesSuggerisci.Risultati.ToString() };

            //RISULTATO GIORNI CORSI
            layout5_risultatoGiorni = initLayout5_risultatoGiorni();
            page5_risultatoGiorni = new ContentPage { Content = layout5_risultatoGiorni, Title = PagesSuggerisci.Risultati.ToString() };

            //page4_risultato = new OrarioCompleto();
            //_completoViewModel = new OrariCompletoViewModel();
            //page4_risultato.BindingContext = _completoViewModel;

            if (!Settings.HelpSuggerisciCorsiHide)       //se ha cliccato su NON MOSTRARE PIU' allora non lo aggiungo
                this.Children.Add(page0_help);          //0

            this.Children.Add(page1_settings);          //1
            this.Children.Add(page2_obbligatori);       //2
            this.Children.Add(page3_scelta);            //3
            this.Children.Add(page4_risultato);         //4
            this.Children.Add(page5_risultatoGiorni);         //5

            _index = 0;

            _listObbligatori = new List<CorsoCompleto>();
            _listScelta = new List<CorsoCompletoAlgoritmo>();
        }

        private StackLayout initLayout0_help()
        {
            var _lblTitle = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Qualche informazione utile!",
                TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            var grid = new Grid()
            {
                Padding = new Thickness(10, 10, 10, 10),
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowSpacing = 8,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Auto) },
                }
            };

            var str1 = "Grazie a un potente algoritmo, UnibgOrari ti suggerisce i corsi da frequentare secondo le tue esigenze!";
            var str2 = "Vorresti evitare sovrapposizioni tra le lezioni oppure preferisci avere un giorno libero a casa?";
            var str3 = "Inserisci il numero di corsi totali che dovrai inserire nel tuo piano di studi.";
            var str4 = "Scegli poi una lista di corsi obbligatori o che vuoi frequentare sicuramente.";
            var str5 = "Infine aggiungi un insieme di corsi a scelta sui quali sei indeciso.";
            var str6 = "UnibgOrari ti restituirà il tuo piano di studi perfetto!";

            var str = string.Format("{0}\n\n{1}\n{2}\n{3}\n{4}", str1, str2, str3, str4, str5, str6);
            grid.Children.Add(new Label() { Text = str, HorizontalOptions = LayoutOptions.Start, FontSize = Device.GetNamedSize(NamedSize.Medium, this), }, 0, 1, 0, 1);
            grid.Children.Add(new Label() { Text = str6, HorizontalOptions = LayoutOptions.CenterAndExpand, FontAttributes = FontAttributes.Bold, FontSize = Device.GetNamedSize(NamedSize.Medium, this), }, 0, 1, 1, 2);

            var scrollView = new ScrollView() { Content = grid};

            _switchHelpHide = new Switch() { IsToggled = false, HorizontalOptions = LayoutOptions.EndAndExpand };
            var lblHelpHide = new Label() { Text = "Non mostrare più", TextColor = ColorHelper.DarkGray };
            var helpHideLayout = new Grid()
            {
                Padding = new Thickness(10, 10, 10, 10),
                VerticalOptions = LayoutOptions.EndAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                RowDefinitions =
                {
                    new RowDefinition { Height = GridLength.Auto },
                },
                ColumnDefinitions =
                {
                    new ColumnDefinition { Width = new GridLength(2, GridUnitType.Star) },
                    new ColumnDefinition { Width = new GridLength(1, GridUnitType.Star) },
                }
            };
            helpHideLayout.Children.Add(lblHelpHide, 0, 1, 0, 1);
            helpHideLayout.Children.Add(_switchHelpHide, 1, 2, 0, 1);

            var btnHelp = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Avanti",
                BackgroundColor = ColorHelper.Blue700,
                TextColor = ColorHelper.White,
                BorderColor = ColorHelper.White,
            };
            btnHelp.Clicked += BtnHelp_Clicked;

            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                BackgroundColor = ColorHelper.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                VerticalOptions = LayoutOptions.FillAndExpand,
                Spacing = 5,
                Children = { _lblTitle, scrollView, helpHideLayout, btnHelp }
            };

            return layout;
        }

        private StackLayout initLayout1_settings()
        {
            //TODO Picker numero di corsi e peso difficoltà
            listFacolta = Facolta.facolta;
            _pickerFacoltà = new Picker()
            {
                Title = "Facoltà"
            };
            _pickerLaurea = new Picker()
            {
                Title = "Corso di Laurea"
            };

            int index = 0;
            foreach (var f in listFacolta)
            {
                _pickerFacoltà.Items.Add(f.Nome);
                if (f.IdFacolta == Settings.FacoltaId && f.DB == Settings.FacoltaDB)
                    _facoltàIndex = index;

                index++;
            }

            //pickerFacoltà.SelectedIndex = facoltàIndex;

            _pickerFacoltà.SelectedIndexChanged += (sender, args) =>
            {
                var s = _pickerFacoltà.Items[_pickerFacoltà.SelectedIndex];
                Facolta facolta = listFacolta.Where(x => x.Nome == s).First();
                //dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                dictionaryLauree = LaureeDictionary.getLauree(facolta).Where(x => x.Value != 0).ToDictionary(x => x.Key, x => x.Value);
                _pickerLaurea.Items.Clear();

                if (dictionaryLauree.Count == 0)
                    return;

                index = 0;
                foreach (var item in dictionaryLauree)
                {
                    _pickerLaurea.Items.Add(item.Key);
                    if (item.Value == Settings.LaureaId)
                        _laureaIndex = index;

                    index++;
                }


                _pickerLaurea.SelectedIndex = 0;
            };

            anni = new String[] {"TUTTI gli anni", "1° Anno", "2° Anno", "3° Anno", "4° Anno", "5° Anno" };

            _pickerLaurea.SelectedIndexChanged += (sender, args) =>
            {
                int i = 0;
                _pickerAnno.IsEnabled = true;

                if (_pickerLaurea.Items.Count > 0)
                {
                    if (_pickerLaurea.Items[_pickerLaurea.SelectedIndex].Contains("Magistrale"))
                        limit = 5;
                    else if (_pickerLaurea.Items[_pickerLaurea.SelectedIndex].Contains("LM"))
                        limit = 2;
                    else limit = 3;

                    _pickerAnno.Items.Clear();
                    foreach (var x in anni)
                    {
                        if (i > limit)
                            break;
                        _pickerAnno.Items.Add(x);
                        i++;
                    }
                    _pickerAnno.SelectedIndex = 0;
                }

            };

            _pickerAnno = new Picker() { Title = "Ordina per..", };

            _pickerSemestre = new Picker() { Title = "Semestre" };
            sem = new Dictionary<string, string>()
            {
                {"Primo", "completo"}, {"Secondo", "secondo"}
            };

            foreach (var x in anni)
                _pickerAnno.Items.Add(x);

            foreach (var s in sem)
                _pickerSemestre.Items.Add(s.Key);

            //pickerFacoltà.SelectedIndex = Settings.FacoltaIndex;
            _pickerFacoltà.SelectedIndex = _facoltàIndex;

            //if (Settings.LaureaIndex == 0)
            //    pickerLaurea.SelectedIndex = 0;
            //else
                _pickerLaurea.SelectedIndex = _laureaIndex;
            //pickerLaurea.SelectedIndex = Settings.LaureaIndex;

            //pickerAnno.SelectedIndex = int.Parse(Settings.Anno) + 1;
            _pickerAnno.SelectedIndex = (int)Settings.AnnoIndex;

            //Se siamo a Marzo, secondo semsestre
            _pickerSemestre.SelectedIndex = DateTime.Today.Month >= new DateTime(2016, 03, 01).Month && DateTime.Today.Month <= new DateTime(2016, 06, 01).Month ? 1 : 0;

            _pickerNumCorsi = new Picker() { Title = "Numero corsi"};
            for (int i = 1; i < 11; i++)
                _pickerNumCorsi.Items.Add(i.ToString());

            _pickerPesi = new Picker() { Title = "Ti pesa andare un giorno in più?"};
            _pickerPesi.Items.Add("Non è un problema!");
            _pickerPesi.Items.Add("Molto! Preferirei evitare");

            lblError = new Label()
            {
                Text = "ORARIO NON DISPONIBILE O IN CORSO DI DEFINIZIONE",
                TextColor = ColorHelper.Red500,
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                HorizontalOptions = LayoutOptions.CenterAndExpand,
                IsVisible = false,
            };
            activityIndicator = new ActivityIndicator()
            {
                IsVisible = false,
                IsRunning = true,
                HorizontalOptions = LayoutOptions.Fill
            };

			var btn = new Button()
			{
				VerticalOptions = LayoutOptions.EndAndExpand,
				Text = "Avanti",
				BackgroundColor = ColorHelper.Blue700,
				TextColor = ColorHelper.White,
				BorderColor = ColorHelper.White,
			};
			btn.Clicked += btnSettings_Clicked;

            var layout = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children = { _pickerFacoltà, _pickerLaurea, _pickerAnno, _pickerSemestre, _pickerNumCorsi, _pickerPesi, lblError, activityIndicator, btn }
            };

            return layout;
        }

        private StackLayout initLayout2_obbligatori()
        {
            var _lblInfo = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Seleziona i corsi obbligatori",
                TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            _listViewObbligatori = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup)),
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
            };

            if (Device.OS != TargetPlatform.WinPhone)
                _listViewObbligatori.GroupHeaderTemplate = new DataTemplate(typeof(HeaderSemestreCell));

            _listViewObbligatori.ItemSelected += _listViewObbligatori_ItemSelected;

            var btnObbligatori = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Avanti",
                BackgroundColor = ColorHelper.Blue700,
                TextColor = ColorHelper.White,
                BorderColor = ColorHelper.White,
            };
            btnObbligatori.Clicked += BtnObbligatori_Clicked;
            //DA AGGIUNGERE A TERZA SCHERMATA!
            //tbiNext = new ToolbarItem("Avanti", "ic_next.png", toolbarItem_next, 0, 0);
            //ToolbarItems.Add(tbiNext);

            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ColorHelper.WhiteSmokeDark,
                Spacing = 5,
                Children = { _lblInfo, _listViewObbligatori, btnObbligatori } //, _activityIndicator }
            };
            return layout;
        }

        private StackLayout initLayout3_scelta()
        {
            var _lblInfo = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Seleziona i corsi a scelta",
                TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand,
            };

            _listViewScelta = new ListView()
            {
                ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup)),
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
            };

            if (Device.OS != TargetPlatform.WinPhone)
                _listViewScelta.GroupHeaderTemplate = new DataTemplate(typeof(HeaderSemestreCell));

            _listViewScelta.ItemSelected += _listViewScelta_ItemSelected;

            var btnScelta = new Button()
            {
                VerticalOptions = LayoutOptions.EndAndExpand,
                Text = "Avanti",
                BackgroundColor = ColorHelper.Blue700,
                TextColor = ColorHelper.White,
                BorderColor = ColorHelper.White,
            };
            btnScelta.Clicked += BtnScelta_Clicked;

            var layout = new StackLayout()
            {
                //BackgroundColor = ColorHelper.White,
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                HorizontalOptions = LayoutOptions.FillAndExpand,
                BackgroundColor = ColorHelper.WhiteSmokeDark,
                Spacing = 5,
                Children = { _lblInfo, _listViewScelta, btnScelta } //, _activityIndicator }
            };
            return layout;
        }

        private AbsoluteLayout initLayout4_risultato()
        {
            fab4 = new FloatingActionButtonView()
            {
                ImageName = "ic_sharee.png",
                ColorNormal = ColorHelper.Blue500,
                ColorPressed = ColorHelper.Blue900,
                ColorRipple = ColorHelper.Blue500,
                Size = FloatingActionButtonSize.Normal,
                Clicked = (sender, args) =>
                {
                    share4();
                }
            };

            var lblOrario = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Ecco l'elenco dei corsi suggeriti!",
                TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            //var lblLaurea = new Label()
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, this),
            //    TextColor = ColorHelper.DarkBlue,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand
            //};
            //lblLaurea.SetBinding(Label.TextProperty, "LaureaString");

            //var lblAnno = new Label()
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, this),
            //    TextColor = ColorHelper.DarkBlue,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand
            //};
            //lblAnno.SetBinding(Label.TextProperty, "AnnoSemestre");

            _lvCorsiSUggerit = new ListView()
            {
                //SCHERMATA DIVISA PER CORSO
                HasUnevenRows = true,
                VerticalOptions = LayoutOptions.FillAndExpand,
                SeparatorColor = Color.Transparent,
                ItemTemplate = new DataTemplate(typeof(OrarioComplCell))

                //SCHERMATA DIVISA PER GIORNO
                //ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup)),
                //HasUnevenRows = true,
                //IsGroupingEnabled = true,
                //GroupDisplayBinding = new Binding("Key"),
            };
            ////lv.

            //lv.ItemSelected += (sender, e) => {
            //    ((ListView)sender).SelectedItem = null;
            //};

            //            lv.ItemSelected += lv_ItemSelected;

            //var searchbar = new SearchBar()
            //{
            //    Placeholder = "Cerca",
            //    VerticalOptions = LayoutOptions.EndAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //};
            //searchbar.TextChanged += searchbar_TextChanged;


            //var l = new StackLayout()
            //{
            //    BackgroundColor = ColorHelper.White,
            //    Padding = new Thickness(15, 10, 15, 10),
            //    Spacing = 5,
            //    Children = { lblOrario } //, lblLaurea, lblAnno }
            //};
            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    //l,
                    lblOrario,
                    _lvCorsiSUggerit,
                    //new StackLayout(){ Children = {searchbar}, BackgroundColor = Color.White}
                }
            };

            //tbiShowFav = new ToolbarItem("Mostra preferiti", "ic_nostar.png", showFavourites, 0, 0);
            //tbiShowAll = new ToolbarItem("Mostra tutti", "ic_star.png", showAll, 0, 0);
            //			tbiShare = new ToolbarItem ("Share", "ic_next.png", share, 0, 1);

            //if (Settings.SuccessLogin)
            //    ToolbarItems.Add(tbiShowFav);

            var absolute = new AbsoluteLayout()
            {
                //Padding = new Thickness(10, 10, 10, 10),
                BackgroundColor = ColorHelper.WhiteSmokeDark,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Position the pageLayout to fill the entire screen.
            // Manage positioning of child elements on the page by editing the pageLayout.
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0f, 0f, 1f, 1f));
            absolute.Children.Add(layout);

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab4, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab4, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absolute.Children.Add(fab4);

            return absolute;
            //return layout;
        }

        private AbsoluteLayout initLayout5_risultatoGiorni()
        {
            fab5 = new FloatingActionButtonView()
            {
                ImageName = "ic_sharee.png",
                ColorNormal = ColorHelper.Blue500,
                ColorPressed = ColorHelper.Blue900,
                ColorRipple = ColorHelper.Blue500,
                Size = FloatingActionButtonSize.Normal,
                Clicked = (sender, args) =>
                {
                    share5();
                }
            };

            var lblOrario = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                Text = "Ecco l'elenco dei corsi suggeriti!",
                TextColor = ColorHelper.Green500,
                HorizontalOptions = LayoutOptions.CenterAndExpand
            };

            //var lblLaurea = new Label()
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, this),
            //    TextColor = ColorHelper.DarkBlue,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand
            //};
            //lblLaurea.SetBinding(Label.TextProperty, "LaureaString");

            //var lblAnno = new Label()
            //{
            //    FontSize = Device.GetNamedSize(NamedSize.Medium, this),
            //    TextColor = ColorHelper.DarkBlue,
            //    HorizontalOptions = LayoutOptions.CenterAndExpand
            //};
            //lblAnno.SetBinding(Label.TextProperty, "AnnoSemestre");

            _lvGiorni = new ListView()
            {
                //SCHERMATA DIVISA PER CORSO
                //HasUnevenRows = true,
                //VerticalOptions = LayoutOptions.FillAndExpand,
                //SeparatorColor = Color.Transparent,
                //ItemTemplate = new DataTemplate(typeof(OrarioComplCell))

                //SCHERMATA DIVISA PER GIORNO
                ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup)),
                HasUnevenRows = true,
                IsGroupingEnabled = true,
                GroupDisplayBinding = new Binding("Key"),
            };

            //lv.ItemSelected += (sender, e) => {
            //    ((ListView)sender).SelectedItem = null;
            //};

            //            lv.ItemSelected += lv_ItemSelected;

            //var searchbar = new SearchBar()
            //{
            //    Placeholder = "Cerca",
            //    VerticalOptions = LayoutOptions.EndAndExpand,
            //    HorizontalOptions = LayoutOptions.FillAndExpand,
            //};
            //searchbar.TextChanged += searchbar_TextChanged;


            //var l = new StackLayout()
            //{
            //    BackgroundColor = ColorHelper.White,
            //    Padding = new Thickness(15, 10, 15, 10),
            //    Spacing = 5,
            //    Children = { lblOrario } //, lblLaurea, lblAnno }
            //};
            var layout = new StackLayout()
            {
                Padding = new Thickness(15, 10, 15, 10),
                Orientation = StackOrientation.Vertical,
                Children =
                {
                    //l,
                    lblOrario,
                    _lvGiorni,
                    //new StackLayout(){ Children = {searchbar}, BackgroundColor = Color.White}
                }
            };

            //tbiShowFav = new ToolbarItem("Mostra preferiti", "ic_nostar.png", showFavourites, 0, 0);
            //tbiShowAll = new ToolbarItem("Mostra tutti", "ic_star.png", showAll, 0, 0);
            //			tbiShare = new ToolbarItem ("Share", "ic_next.png", share, 0, 1);

            //if (Settings.SuccessLogin)
            //    ToolbarItems.Add(tbiShowFav);

            var absolute = new AbsoluteLayout()
            {
                //Padding = new Thickness(10, 10, 10, 10),
                BackgroundColor = ColorHelper.WhiteSmokeDark,
                VerticalOptions = LayoutOptions.FillAndExpand,
                HorizontalOptions = LayoutOptions.FillAndExpand
            };

            // Position the pageLayout to fill the entire screen.
            // Manage positioning of child elements on the page by editing the pageLayout.
            AbsoluteLayout.SetLayoutFlags(layout, AbsoluteLayoutFlags.All);
            AbsoluteLayout.SetLayoutBounds(layout, new Rectangle(0f, 0f, 1f, 1f));
            absolute.Children.Add(layout);

            // Overlay the FAB in the bottom-right corner
            AbsoluteLayout.SetLayoutFlags(fab5, AbsoluteLayoutFlags.PositionProportional);
            AbsoluteLayout.SetLayoutBounds(fab5, new Rectangle(1f, 1f, AbsoluteLayout.AutoSize, AbsoluteLayout.AutoSize));
            absolute.Children.Add(fab5);

            return absolute;
            //return layout;
        }

        private async void share4()
        {
            string text;

            var s = await DisplayActionSheet("Condividi", "Annulla", null, "Visualizza PDF", "Condividi PDF", "Condividi Testo");
            if (s.Contains("PDF"))
            { //devo creare il PDF

                //if (_viewModel.Group)
                //{
                //    ListGroupToString();
                //    text = _listStringGroup;  //lista corsi raggruppati
                //}
                //else
                text = string.Join("\n", _viewModel.ListOrari); //lista corsi

                PdfFile pdf = new PdfFile() { Title = "Elenco corsi suggeriti", TitleFacolta = _viewModel.LaureaString, TitleInfo = _viewModel.AnnoSemestre, Text = text };
                pdf.CreateCompleto();

                await pdf.Save();
                if (s.Contains("Condividi")) //Condividi PDF
                    DependencyService.Get<IFile>().Share(pdf._filename);
                else
                {
                    resetView = false;  //non voglio resettare le view
                    await pdf.Display(); //visualizza PDF
                }

            }
            else
            {
                //if (_viewModel.Group)
                //    text = ListGroupToString();
                //else
                    text = _viewModel.ToString();
                text += Settings.Firma;
                DependencyService.Get<IMethods>().Share(text); //condividi testo
            }
        }

        private async void share5()
        {
            string text;

            var s = await DisplayActionSheet("Condividi", "Annulla", null, "Visualizza PDF", "Condividi PDF", "Condividi Testo");
            if (s.Contains("PDF"))
            { //devo creare il PDF

                ListGroupToString();
                text = _listStringGroup;  //lista corsi raggruppati

                PdfFile pdf = new PdfFile() { Title = "Orario settimanale corsi suggeriti", TitleFacolta = _viewModel.LaureaString, TitleInfo = _viewModel.AnnoSemestre, Text = text };
                pdf.CreateCompleto();

                await pdf.Save();
                if (s.Contains("Condividi")) //Condividi PDF
                    DependencyService.Get<IFile>().Share(pdf._filename);
                else
                {
                    resetView = false;  //non voglio resettare le view
                    await pdf.Display(); //visualizza PDF
                }
            }
            else
            {
                text = ListGroupToString();

                text += Settings.Firma;
                DependencyService.Get<IMethods>().Share(text); //condividi testo
            }
        }

        private void setUpListView()
        {
            //if (!grouped) //corsi 
            //{
            //corsi (ogni corso elenco lezioni)
                _lvCorsiSUggerit.ItemsSource = _listSuggeriti;
            //}
            //else        //corsi raggruppati per giorno
            //{
            //corsi raggruppati per giorno
            listaGroup = from corso in _listSuggeriti
                                 from lez in corso.Lezioni
                                 orderby lez.Ora
                                 where lez.AulaOra != string.Empty
                                 group new CorsoCompletoGroupViewModel() { Insegnamento = corso.Insegnamento, Docente = corso.Docente, Codice = corso.Codice, InizioFine = corso.InizioFine, AulaOra = lez.AulaOra, IsVisible = lez.isVisible, Giorno = lez.Giorno, Note = lez.Note, Day = lez.day } by lez.Giorno into Group
                                 // group new { corso.Insegnamento, corso.Docente, Cod = corso.Codice, corso.InizioFine, lez.AulaOra, lez.Aula, lez.Ora, lez.isVisible, lez.Giorno, lez.Note, lez.day } by lez.Giorno into Group
                                 //group corso by lez.Giorno into Group
                                 select Group;

            listaGroup = listaGroup.OrderBy(x => (int)x.Key);
            _lvGiorni.ItemsSource = listaGroup;

            //lvGiorni.ItemTemplate = new DataTemplate(typeof(OrarioComplCellGroup));
            //lvGiorni.IsGroupingEnabled = true;
            //lvGiorni.GroupDisplayBinding = new Binding("Key");
            if (Device.OS != TargetPlatform.WinPhone)
                _lvGiorni.GroupHeaderTemplate = new DataTemplate(typeof(HeaderCell));
            //}
        }

        private string ListGroupToString()
        {
            string text = string.Format("ORARIO COMPLETO CORSI A SCELTA: {0} - {1} \n\n", _viewModel.LaureaString, _viewModel.AnnoSemestre);
            var days = Enum.GetValues(typeof(OrariUnibg.Models.Lezione.Day));

            _listStringGroup = string.Empty;
            foreach (var day in days)
            {
                var list = listaGroup.Where(z => z.Key.ToString() == day.ToString()).SelectMany(value => value);
                if (list.Count() > 0)
                    _listStringGroup += string.Format("{0}\n{1}\n\n", day.ToString().ToUpper(), string.Join("\n", list));
            }

            return text += _listStringGroup;
        }
        #endregion


        #region Event Handlers

        void _listViewObbligatori_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var selected = (CorsoCompleto)_listViewObbligatori.SelectedItem;

            int index = _listObbligatori.FindIndex(f => f.Insegnamento == selected.Insegnamento);

            if (index >= 0)
            {
                MessagingCenter.Send<SuggerisciCorsiView, CorsoCompleto>(this, "deselect_obb", selected);
                _listObbligatori.RemoveAt(index);
            }
            else
            {
                MessagingCenter.Send<SuggerisciCorsiView, CorsoCompleto>(this, "select_obb", selected);
                _listObbligatori.Add(selected);
            }

            ((ListView)sender).SelectedItem = null;
        }

        void _listViewScelta_ItemSelected(object sender, SelectedItemChangedEventArgs e)
        {
            if (e.SelectedItem == null)                         // ensures we ignore this handler when the selection is just being cleared
                return;
            var selected = (CorsoCompleto)_listViewScelta.SelectedItem;

            int index = _listScelta.FindIndex(f => f.Insegnamento == selected.Insegnamento);
            if (index >= 0)
            {
                MessagingCenter.Send<SuggerisciCorsiView, CorsoCompleto>(this, "deselect_scelta", selected);
                _listScelta.RemoveAt(index);
            }
            else
            {
                MessagingCenter.Send<SuggerisciCorsiView, CorsoCompleto>(this, "select_scelta", selected);
                var corso = new CorsoCompletoAlgoritmo()
                {
                    Insegnamento = selected.Insegnamento,
                    Codice = selected.Codice,
                    Docente = selected.Docente,
                    Lezioni = selected.Lezioni,
                    Id = selected.Id,
                    InizioFine = selected.InizioFine,
                    Punteggio = 0 };

                _listScelta.Add(corso);
            }

            ((ListView)sender).SelectedItem = null;
        }


        private void BtnHelp_Clicked(object sender, EventArgs e)
        {
            Settings.HelpSuggerisciCorsiHide = _switchHelpHide.IsToggled;
            //_index++;
            //this.CurrentPage = this.Children[_index];

            _index = this.Children.IndexOf(page1_settings);
            this.CurrentPage = this.Children[_index];
        }

        private async void btnSettings_Clicked(object sender, EventArgs e)
        {
            lblError.IsVisible = false;

            _fac = listFacolta.Where(x => x.Nome == _pickerFacoltà.Items[_pickerFacoltà.SelectedIndex]).First();
            int facolta = _fac.IdFacolta;
            string db = _fac.DB;
            int laureaId = dictionaryLauree.Where(x => x.Key == _pickerLaurea.Items[_pickerLaurea.SelectedIndex]).First().Value;
            _laurea = dictionaryLauree.Where(x => x.Key == _pickerLaurea.Items[_pickerLaurea.SelectedIndex]).First().Key;
            int anno = _pickerAnno.SelectedIndex;
            _annoString = anni[anno];
            string semestre = sem.Where(x => x.Key == _pickerSemestre.Items[_pickerSemestre.SelectedIndex]).First().Value;
            _semestreString = sem.Where(x => x.Key == _pickerSemestre.Items[_pickerSemestre.SelectedIndex]).First().Key;

            NUM_CORSI =_pickerNumCorsi.SelectedIndex + 1; 

            int peso = _pickerPesi.SelectedIndex;
            if(peso == 0)
            {
                PESO_GIORNO = 1;
                PESO_SOVRAPPOSIZIONE = 5;
            }
            else
            {
                PESO_GIORNO = 10;
                PESO_SOVRAPPOSIZIONE = 1;
            }

            if (!CrossConnectivity.Current.IsConnected)
            { //non connesso a internet
                //activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                var toast = DependencyService.Get<IToastNotificator>();
                await toast.Notify(ToastNotificationType.Error, "Errore", "Nessun accesso a internet", TimeSpan.FromSeconds(3));
                return;
            }

            if(_pickerNumCorsi.SelectedIndex == -1 || _pickerPesi.SelectedIndex == -1)
            {
                var toast = DependencyService.Get<IToastNotificator>();
                await toast.Notify(ToastNotificationType.Warning, "Attenzione", "Devi completare tutti i campi", TimeSpan.FromSeconds(3));
                //activityIndicator.IsVisible = false;
                return;
            }

            activityIndicator.IsVisible = true;

            string s = await Web.GetOrarioCompleto(semestre, db, facolta, laureaId, anno);

            if (s == string.Empty)
            {
                activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                return;
            }

            _listSource = Web.GetSingleOrarioCompleto(s);

            if (_listSource.Count() == 0)
            {
                activityIndicator.IsVisible = false;
                lblError.IsVisible = true;
                return;
            }

            _listaGroup = _listSource.GroupBy(x => x.Semestre);

            var _listaGroupObb = _listaGroup;
            _listViewObbligatori.ItemsSource = _listaGroupObb;

            var _listaGroupSce = _listSource.GroupBy(x => x.Semestre);
            _listViewScelta.ItemsSource = _listaGroupSce;

            activityIndicator.IsVisible = false;

            _viewModel = new OrariCompletoViewModel() { Facolta = _fac, LaureaString = _laurea, Anno = _annoString, Semestre = _semestreString, Group = false };

            //_index++;
            //this.CurrentPage = this.Children[_index];

            _index = this.Children.IndexOf(page2_obbligatori);
            this.CurrentPage = this.Children[_index];

            //List<CorsoCompleto> lista = Web.GetSingleOrarioCompleto(s);
            //activityIndicator.IsVisible = false;
        }

        private void BtnObbligatori_Clicked(object sender, EventArgs e)
        {
            var _listGroupScelta = _listSource.Except(_listObbligatori).ToList();

            _listViewScelta.ItemsSource = _listGroupScelta.GroupBy(x => x.Semestre);
            //_index++;
            //this.CurrentPage = this.Children[_index];
            //this.CurrentPage = this.Children[2];

            _index = this.Children.IndexOf(page3_scelta);
            this.CurrentPage = this.Children[_index];
        }

        private void BtnScelta_Clicked(object sender, EventArgs e)
        {
            _listSuggeriti = AlgoritmoCorsi.SuggerisciCorsi(_listObbligatori, _listScelta, NUM_CORSI, PESO_GIORNO, PESO_SOVRAPPOSIZIONE).ToList();
            _lvCorsiSUggerit.ItemsSource = null;
            _lvGiorni.ItemsSource = null; //resetta lista

            _viewModel.ListOrari = _listSuggeriti;
            //bool grouped = false;                            //corsi (ogni corso elenco lezioni)
            setUpListView();

            //_index++;
            //_index = this.Children.Where(x => x.Title == PagesSuggerisci.Risultati.ToString()).First());
            _index = this.Children.IndexOf(page4_risultato);
            this.CurrentPage = this.Children[_index];
            //this.CurrentPage = this.Children[3];
        }

        protected override bool OnBackButtonPressed()
        {
            return true; //ALERT CHIUDERE APP??
        }

        #endregion

        #region Overrides
        protected override void OnAppearing()
        {
            base.OnAppearing();
            if(resetView)
                this.CurrentPage = this.Children[0];

            resetView = true;
            //lv.ItemAppearing += List_ItemAppearing;
            //lv.ItemDisappearing += List_ItemDisappearing;
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();

            if (resetView)
            {
                _lvCorsiSUggerit.ItemsSource = null;
                _lvGiorni.ItemsSource = null;
                _listViewObbligatori.ItemsSource = null;
                _listViewScelta.ItemsSource = null;
            }
            //lv.ItemAppearing -= List_ItemAppearing;
            //lv.ItemDisappearing -= List_ItemDisappearing;
        }

        //async void List_ItemDisappearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    await Task.Run(() =>
        //    {
        //        var items = lv.ItemsSource as IList;
        //        if (items != null)
        //        {
        //            var index = items.IndexOf(e.Item);
        //            if (index < appearingListItemIndex)
        //            {
        //                Device.BeginInvokeOnMainThread(() => fab5.Hide());
        //            }
        //            appearingListItemIndex = index;
        //        }
        //    });
        //}

        //async void List_ItemAppearing(object sender, ItemVisibilityEventArgs e)
        //{
        //    await Task.Run(() =>
        //    {
        //        var items = lv.ItemsSource as IList;
        //        if (items != null)
        //        {
        //            var index = items.IndexOf(e.Item);
        //            if (index < appearingListItemIndex || index == 0)
        //            {
        //                Device.BeginInvokeOnMainThread(() => fab5.Show());
        //            }
        //            appearingListItemIndex = index;
        //        }
        //    });
        //}
        #endregion
    }

    public enum PagesSuggerisci
    {
        Help,
        Settings,
        Obbligatori,
        Scelta,
        Risultati,
        RisultatiGiorni
    }
}
