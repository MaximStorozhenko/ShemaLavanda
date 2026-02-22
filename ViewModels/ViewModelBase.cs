using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Navigation;

namespace ShemaLavanda.ViewModels
{
    internal class ViewModelBase : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler PropertyChanged = delegate { };

        public void Set<T>(ref T field, T value, [CallerMemberName] string propName = "")
        {
            if (!field.Equals(value))
                field = value;

            PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }

        public void Notify(params string[] names)
        {
            foreach (var name in names)
            {
                PropertyChanged(this, new PropertyChangedEventArgs(name));
            }
        }
    }
}
