using AdminPanel.Services.Local;
using AdminPanel.ViewModels.Core;
using ContractLibrary.DataTransfer.Dto;
using System.Collections.ObjectModel;

namespace AdminPanel.ViewModels
{
    
    internal sealed class MonitorViewModel : BaseViewModel
    {
        private ObservableCollection<ActionDto> _operationList;

        public MonitorViewModel(IMessageService<bool> messageService) : base(messageService)
        {
            _operationList = new ObservableCollection<ActionDto>();
        }
        public ObservableCollection<ActionDto> OperationList
        {
            get { return _operationList; }
        }
    }
}
