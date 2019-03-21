using System;
using Microsoft.Win32;

namespace Organizer.Services.FileServises
{
    internal abstract class FileService<T> : IFileDialogService<T> where T : class
    {
        protected OpenFileDialog _openFileDialog = new OpenFileDialog();
        protected SaveFileDialog _saveFileDialog = new SaveFileDialog();

        public FileService()
        {
            InitOpenFileDialog();
            InitSafeFileDialog();
        }

        public string OpenFileExtension
        {
            set
            {
                _openFileDialog.Filter = value;
            }
        }
        public string SafeFileExtension
        {
            set
            {
                _saveFileDialog.Filter = value;
            }
        }
        public T OpenFileDialog()
        {
            var dlgResult = _openFileDialog.ShowDialog();

            if (dlgResult == true)
            {
                return GetResult();
            }

            return null;
        }
        public string SaveFileDialog()
        {
            var dlgResult = _saveFileDialog.ShowDialog();

            if (dlgResult == true)
            {
                return _saveFileDialog.FileName;
            }

            return null;
        }
        protected abstract T GetResult();
        protected virtual void InitOpenFileDialog() { }
        protected virtual void InitSafeFileDialog() { }
        public void Dispose()
        {
            OpenFileExtension = "Все файлы |*.*";
            SafeFileExtension = "Все файлы |*.*";
        }
        public void Initialize(Action<OpenFileDialog> initAction)
        {
            throw new NotImplementedException();
        }
    }
}
