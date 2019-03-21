using ContractLibrary.DataTransfer.Dto;
using System.Collections.ObjectModel;

namespace AdminPanel.Services.Net
{
    internal class NotifyServiceConfiguration
    {
        public ObservableCollection<ActionDto> ActionList { get; set; }
        public ObservableCollection<UserDto> UserList { get; set; }
    }
}
