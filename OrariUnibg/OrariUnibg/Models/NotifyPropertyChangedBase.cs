using System;
using System.Collections.Generic;
using System.ComponentModel;

namespace OrariUniBg.Models
{
    public class NotifyPropertyChangedBase : INotifyPropertyChanged
    {
        protected bool SetProperty<T>(ref T property, T value, string propertyName)
        {
            if (EqualityComparer<T>.Default.Equals(property, value))
                return false;

            property = value;
            OnPropertyChanged(propertyName);
            return true;
        }

        private void OnPropertyChanged(string propertyName)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}

