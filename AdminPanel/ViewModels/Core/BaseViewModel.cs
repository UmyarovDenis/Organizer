using AdminPanel.Services.Local;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace AdminPanel.ViewModels.Core
{
    internal class BaseViewModel : INotifyPropertyChanged
    {
        private IMessageService<bool> _messageService;

        public BaseViewModel(IMessageService<bool> messageService)
        {
            MessageService = messageService;
        }
        protected IMessageService<bool> MessageService
        {
            get { return _messageService; }
            set
            {
                _messageService = value;
            }
        }
        protected virtual void RaisePropertyChanged([CallerMemberName]string proprtyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(proprtyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
