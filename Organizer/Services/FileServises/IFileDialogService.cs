using System;

namespace Organizer.Services.FileServises
{
    internal interface IFileDialogService<out T> : IDisposable where T : class
    {
        T OpenFileDialog();
        string SaveFileDialog();
        string OpenFileExtension { set; }
        string SafeFileExtension { set; }
    }
}
