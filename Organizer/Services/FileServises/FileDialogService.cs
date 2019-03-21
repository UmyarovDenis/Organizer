namespace Organizer.Services.FileServises
{
    internal class FileDialogService : FileService<string>
    {
        public FileDialogService() : base()
        { }
        protected override string GetResult()
        {
            return _openFileDialog.FileName;
        }
    }
}
