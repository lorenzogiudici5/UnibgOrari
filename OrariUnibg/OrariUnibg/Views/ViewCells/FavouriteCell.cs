﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views.ViewCells
{
    class FavouriteCell : ViewCell
    {
        #region Constructor
        public FavouriteCell()
        {
            View = getView();
        }
        #endregion

        #region Private Fields
        private Label _lblInsegn;
        private Label _lblDocente;
        #endregion

        #region Private Methods
        private Xamarin.Forms.View getView()
        {
            _lblInsegn = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Medium, this),
                FontAttributes = FontAttributes.Bold
            };
            _lblInsegn.SetBinding(Label.TextProperty, "Insegnamento");
            
            _lblDocente = new Label()
            {
                FontSize = Device.GetNamedSize(NamedSize.Small, this),
            };
            _lblDocente.SetBinding(Label.TextProperty, "Docente");

            var layout = new StackLayout()
            {
                Children = { _lblInsegn, _lblDocente}
            };

            return layout;
        }
        #endregion
    }
}
