using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace HostManager.ViewModels.Core
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        protected virtual void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
