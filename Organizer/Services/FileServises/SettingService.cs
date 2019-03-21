using Organizer.Utils;
using System;
using System.IO;

namespace Organizer.Services.FileServises
{
    internal class SettingService : ISettingService
    {
        private string _folderSettings;
        private string _mailSettingsFileName;

        public SettingService()
        {
            _folderSettings = "Settings";
            _mailSettingsFileName = "MailSettings.mconf";
        }
        public string FolderSettings
        {
            get { return _folderSettings; }
        }
        public string MailSettingsFileName
        {
            get { return _mailSettingsFileName; }
        }
        public TSettings TryGetSettings<TSettings>(string fileName) where TSettings : class
        {
            try
            {
                return Formatter.GetSettings<TSettings>(Path.Combine(FolderSettings, fileName));
            }
            catch(Exception ex)
            {
                var s = ex.Message;
                return null;
            }
        }
        public void SaveSettings<TSettings>(TSettings settings, string fileName) where TSettings : class
        {
            if (!Directory.Exists(FolderSettings))
            {
                Directory.CreateDirectory(FolderSettings);
            }

            Formatter.SaveSettings<TSettings>(settings, Path.Combine(FolderSettings, fileName));
        }
    }
}
