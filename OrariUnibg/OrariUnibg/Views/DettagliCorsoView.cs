using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUnibg.Views
{
    class DettagliCorsoView : ContentPage
    {
        #region Constructor
        public DettagliCorsoView()
        {
            //this.SetBinding(ContentPage.TitleProperty, "FacoltaString");
            Content = getView();
        }
        #endregion

        #region Private Fields
        //private Label _lblInsegnamento;
        //private Label _lblDocente;
        //private Label _lblCodice;
        #endregion

        #region Private Methods
        private View getView()
        {
            throw new NotImplementedException();
        }
        #endregion
    }
}
