using Microsoft.Win32;

namespace Organizer.Services.FileServises
{
    internal sealed class FileObject
    {
        public string[] FileNames { get; set; }
        public string[] SafeFileNames { get; set; }
    }
    internal class MultiselectFileService : FileService<FileObject>
    {
        protected override FileObject GetResult()
        {
            return new FileObject
            {
                FileNames = _openFileDialog.FileNames,
                SafeFileNames = _openFileDialog.SafeFileNames
            };
        }
        protected override void InitOpenFileDialog()
        {
            _openFileDialog.Multiselect = true;
        }
    }
}
