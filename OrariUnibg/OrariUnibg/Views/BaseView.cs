using OrariUniBg.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace OrariUniBg.Views
{
    public class BaseView : ContentPage
    {
        public BaseView()
        {
            SetBinding(Page.TitleProperty, new Binding(BaseViewModel.TitlePropertyName));
        }
    }
}
