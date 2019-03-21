using Organizer.Models;
using Organizer.Services.FileServises;
using Organizer.Services.Local;
using Organizer.ViewModels.Core;
using System.Collections.ObjectModel;
using System.Windows.Input;

namespace Organizer.ViewModels
{
    internal sealed class SendMailViewModel : BaseViewModel
    {
        private IFileDialogService<FileObject> _fileService;
        private MailMessageDto _mailObject;
        private readonly ObservableCollection<string> _fileNames;

        private ICommand _openFileDialogCommand;
        private ICommand _deleteFileFromListCommand;

        public SendMailViewModel(IMessageService<bool> messageService, IFileDialogService<FileObject> fileDialogService) 
            : base(messageService)
        {
            _mailObject = new MailMessageDto();

            _fileService = fileDialogService;
            _fileNames = new ObservableCollection<string>();
        }
        public MailMessageDto MailObject
        {
            get { return _mailObject; }
        }
        public ObservableCollection<string> FileNames
        {
            get { return _fileNames; }
        }
        public ICommand OpenFileDialogCommand
        {
            get
            {
                return _openFileDialogCommand ??
                    (_openFileDialogCommand = new RelayCommand(arg => OpenFileDialog()));
            }
        }
        public ICommand DeleteFileFromListCommand
        {
            get
            {
                return _deleteFileFromListCommand ??
                    (_deleteFileFromListCommand = new RelayCommand(arg => DeleteFileFromList((int)arg)));
            }
        }
        private void OpenFileDialog()
        {
            FileObject fileObject = _fileService.OpenFileDialog();

            if(fileObject?.FileNames.Length > 0)
            {
                for(int i = 0; i < fileObject.FileNames.Length; i++)
                {
                    if (!MailObject.Attachments.Contains(fileObject.FileNames[i]))
                    {
                        MailObject.Attachments.Add(fileObject.FileNames[i]);
                        FileNames.Add(fileObject.SafeFileNames[i]);
                    }
                }
            }
        }
        private void DeleteFileFromList(int index)
        {
            if(index >= 0)
            {
                MailObject.Attachments.RemoveAt(index);
                FileNames.RemoveAt(index);
            }
        }
    }
}
