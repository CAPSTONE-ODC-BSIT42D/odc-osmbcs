using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace prototype2
{
    /// <summary>
    /// Standard viewmodel class base, simply allows
    /// property change notifications to be sent.
    /// </summary>
    public class ViewModelEntity : INotifyPropertyChanged
    {
        ///// <summary>
        ///// The property changed event.
        ///// </summary>
        //public event PropertyChangedEventHandler PropertyChanged;

        ///// <summary>
        ///// Raises the property changed event.
        ///// </summary>
        ///// <param name="propertyName">Name
        /////              of the property.</param>
        //public virtual void NotifyPropertyChanged(string propertyName)
        //{
        //    //  If the event has been subscribed to, fire it.
        //    if (PropertyChanged != null)
        //        PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propName = null)
        {
            var handler = PropertyChanged;
            if (handler != null)
            {
                handler(this, new PropertyChangedEventArgs(propName));
            }
        }

        protected bool SetProperty<T>(ref T storage, T value,
            [CallerMemberName] String propertyName = null)
        {
            if (Equals(storage, value)) return false;
            storage = value;
            OnPropertyChanged(propertyName);
            return true;
        }
    }
}
