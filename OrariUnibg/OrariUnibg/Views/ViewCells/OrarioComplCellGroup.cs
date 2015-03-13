﻿using System;
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
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                TextColor = ColorHelper.Blue,
            };

            _lblDocente = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
                FontAttributes = Xamarin.Forms.FontAttributes.Bold,
                TextColor = Color.Black,
                HorizontalOptions = LayoutOptions.EndAndExpand,
            };

            _lblAulaOra = new Label()
            {
                TextColor = ColorHelper.Gray,
                FontSize = Device.GetNamedSize(NamedSize.Small, this)
            };

            _lblInsegnamento.SetBinding(Label.TextProperty, "Insegnamento");
            _lblDocente.SetBinding(Label.TextProperty, "Docente");
            //lblInizioFine.SetBinding(Label.TextProperty, "InizioFine");

            _lblAulaOra.SetBinding(Label.TextProperty, "AulaOra");

            MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "select_fav", (sender, arg) =>
            {
                if(arg.Insegnamento == _lblInsegnamento.Text)
                    _layout.BackgroundColor = ColorHelper.LightBlue;
            });
            MessagingCenter.Subscribe<ListaCorsi, MieiCorsi>(this, "deselect_fav", (sender, arg) =>
            {
                if (arg.Insegnamento == _lblInsegnamento.Text)
                    _layout.BackgroundColor = ColorHelper.White;
            });

            _layout = new StackLayout()
            {
                BackgroundColor = ColorHelper.White,
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
