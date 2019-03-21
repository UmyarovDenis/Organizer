using AppController;
using AppController.Infrastructure.Attributes;
using ContractLibrary.DataTransfer.Dto;
using Organizer.Enums;
using Organizer.Infrastructure.Modules;
using Organizer.Services.Local;
using Organizer.Services.Net;
using Organizer.Utils;
using Organizer.ViewModels.Core;
using Organizer.Views;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal sealed class ProjectViewModel : BaseViewModel
    {
        [Injected(typeof(ServiceModule))]
        private IControllerCore _controller;

        private IDataServiceProxy _dataProxy;
        private IImageManager<ImageType> _imageManager;

        private OrganizationDto _selectedOrganization;
        private TaskDto _selectedTask;

        private string _currentTime;
        private int _projectCount;
        private int _comletedProject;
        private int _projectInWork;
        private int _projectOnCheck;
        private ProjectStatus? _status;

        private ICommand _addNewTaskCommand;
        private ICommand _changeTaskCommand;
        private ICommand _removeTaskCommand;

        private ObservableCollection<TaskDto> _tasks;

        public ProjectViewModel(IMessageService<bool> messageService, IDataServiceProxy dataServiceProxy,
            IImageManager<ImageType> imageManager) : base(messageService)
        {
            _dataProxy = dataServiceProxy;
            _imageManager = imageManager;
        }
        public void UpdateOrganizationInfo(OrganizationDto organization)
        {
            SelectedOrganization = organization;
            SelectedTask = null;
            CurrentTime = DateTime.Now.ToShortTimeString();
            ProjectCount = organization.Tasks.Count;
            ProjectsInWork = organization.Tasks.Where(x => x.Status == 1).Count();
            ProjectsOnCheck = organization.Tasks.Where(x => x.Status == 2).Count();
            CompletedProjects = organization.Tasks.Where(x => x.Status == 3).Count();
            Tasks = new ObservableCollection<TaskDto>(organization.Tasks);
        }

        #region BindingProperties

        public ObservableCollection<TaskDto> Tasks
        {
            get { return _tasks; }
            set
            {
                _tasks = value;
                RaisePropertyChanged();
            }
        }
        public TaskDto SelectedTask
        {
            get { return _selectedTask; }
            set
            {
                _selectedTask = value;
                CurrentStatus = value?.Status.To<ProjectStatus>();
                RaisePropertyChanged();
            }
        }
        public ProjectStatus? CurrentStatus
        {
            get { return _status; }
            set
            {
                _status = value;
                RaisePropertyChanged();
            }
        }
        public OrganizationDto SelectedOrganization
        {
            get { return _selectedOrganization; }
            set
            {
                _selectedOrganization = value;
                RaisePropertyChanged();
            }
        }
        public string CurrentTime
        {
            get { return _currentTime; }
            set
            {
                _currentTime = value;
                RaisePropertyChanged();
            }
        }
        public int ProjectCount
        {
            get { return _projectCount; }
            set
            {
                _projectCount = value;
                RaisePropertyChanged();
            }
        }
        public int CompletedProjects
        {
            get { return _comletedProject; }
            set
            {
                _comletedProject = value;
                RaisePropertyChanged();
            }
        }
        public int ProjectsInWork
        {
            get { return _projectInWork; }
            set
            {
                _projectInWork = value;
                RaisePropertyChanged();
            }
        }
        public int ProjectsOnCheck
        {
            get { return _projectOnCheck; }
            set
            {
                _projectOnCheck = value;
                RaisePropertyChanged();
            }
        }

        #endregion

        #region Commands

        public ICommand AddNewTaskCommand
        {
            get
            {
                return _addNewTaskCommand ??
                    (_addNewTaskCommand = new RelayCommand(arg => AddNewTask(), arg => CanAddNewTask()));
            }
        }
        public ICommand ChangeTaskCommand
        {
            get
            {
                return _changeTaskCommand ??
                    (_changeTaskCommand = new RelayCommand(arg => ChangeTask(arg as TaskDto), arg => CanChangeOrRemoveTask()));
            }
        }
        public ICommand RemoveTaskCommand
        {
            get
            {
                return _removeTaskCommand ??
                    (_removeTaskCommand = new RelayCommand(arg => RemoveTask(arg as TaskDto), arg => CanChangeOrRemoveTask()));
            }
        }

        #endregion

        #region CommandImplementations

        private void AddNewTask()
        {
            try
            {
                TaskDto newTask = _controller.GetConfigs<TaskCardView, TaskCardViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.TaskImage))
                    .RunDialogWithResult(r => r.Task);

                if (newTask != null)
                {
                    newTask.Organization_Id = SelectedOrganization.Id;
                    newTask.Status = 1;

                    var result = _dataProxy.CreateItems(newTask);

                    if(result?.Count > 0)
                    {
                        newTask = result.First();
                        newTask.Organization = SelectedOrganization;
                        SelectedOrganization.Tasks.Add(newTask);
                        Tasks.Add(newTask);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void ChangeTask(TaskDto task)
        {
            try
            {
                bool? result = _controller.GetConfigs<TaskCardView, TaskCardViewModel>()
                    .View(v => v.Icon = _imageManager.GetImage(ImageType.TaskImage))
                    .AdditionalViewModelParams(task)
                    .RunDialog();

                if (result == true)
                {
                    task.Organization = null;
                    _dataProxy.UpdateItems(task);
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private void RemoveTask(TaskDto task)
        {
            try
            {
                bool result = MessageService.ShowQuestion("Удалить запись?");

                if (result)
                {
                    task.Organization = null;
                    bool resutl = _dataProxy.RemoveItems(task);

                    if(result == true)
                    {
                        SelectedOrganization.Tasks.Remove(task);
                        Tasks.Remove(task);
                        SelectedTask = null;
                    }
                }
            }
            catch (Exception ex)
            {
                MessageService.ShowError(ex.Message);
            }
        }
        private bool CanAddNewTask()
        {
            return SelectedOrganization != null;
        }
        private bool CanChangeOrRemoveTask()
        {
            return SelectedTask != null;
        }

        #endregion
    }
}
