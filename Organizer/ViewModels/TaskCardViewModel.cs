using ContractLibrary.DataTransfer.Dto;
using Organizer.Services.Local;
using Organizer.ViewModels.Core;

namespace Organizer.ViewModels
{
    internal class TaskCardViewModel : BaseViewModel
    {
        private TaskDto _task;
        private string _taskLabel;
        private string _buttonAddChangeLabel;

        public TaskCardViewModel(IMessageService<bool> messageService) : base(messageService)
        {
            _task = new TaskDto();
            TaskLabel = "Новый проект";
            ButtonLabel = "Добавить";
        }
        public TaskCardViewModel(IMessageService<bool> messageService, TaskDto task) : base(messageService)
        {
            Task = task;
            TaskLabel = task.Label;
            ButtonLabel = "Изменить";
        }
        public TaskDto Task
        {
            get { return _task; }
            set
            {
                _task = value;
                RaisePropertyChanged();
            }
        }
        public string TaskLabel
        {
            get { return _taskLabel; }
            set
            {
                _taskLabel = value;
                RaisePropertyChanged();
            }
        }
        public string ButtonLabel
        {
            get { return _buttonAddChangeLabel; }
            set
            {
                _buttonAddChangeLabel = value;
                RaisePropertyChanged();
            }
        }
    }
}
