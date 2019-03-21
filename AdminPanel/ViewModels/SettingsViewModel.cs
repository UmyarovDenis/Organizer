using AdminPanel.Services.Local;
using AdminPanel.ViewModels.Core;

namespace AdminPanel.ViewModels
{
    internal sealed class SettingsViewModel : BaseViewModel
    {
        public SettingsViewModel(IMessageService<bool> messageService) : base(messageService)
        {

        }
    }
}
