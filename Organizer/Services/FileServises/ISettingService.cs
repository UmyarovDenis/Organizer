namespace Organizer.Services.FileServises
{
    internal interface ISettingService
    {
        string FolderSettings { get; }
        string MailSettingsFileName { get; }
        TSettings TryGetSettings<TSettings>(string fileName) where TSettings : class;
        void SaveSettings<TSettings>(TSettings settings, string fileName) where TSettings : class;
    }
}
