using Organizer.Services.FileServises;
using Organizer.Services.Local;
using Organizer.Settings;
using Organizer.Utils;
using Organizer.ViewModels.Core;
using System.Net.Mail;

namespace Organizer.ViewModels
{
    internal class MailServiceSettingsViewModel : BaseViewModel
    {
        private ISettingService _settingsService;
        private MailSettings _mailSettings;
        private DeliveryMethod _deliveryMethod;

        public MailServiceSettingsViewModel(IMessageService<bool> messageService, ISettingService settingService)
            : base(messageService)
        {
            _settingsService = settingService;
            _mailSettings = settingService.TryGetSettings<MailSettings>(settingService.MailSettingsFileName);

            if(_mailSettings == null)
            {
                _mailSettings = new MailSettings();
            }
            else
            {
                DeliveryMethod = _mailSettings.DeliveryMethod.To<DeliveryMethod>();
            }
        }
        public MailSettings MailSettings
        {
            get { return _mailSettings; }
        }
        public DeliveryMethod DeliveryMethod
        {
            get { return _deliveryMethod; }
            set
            {
                _deliveryMethod = value;
                MailSettings.DeliveryMethod = value.To<SmtpDeliveryMethod>();
            }
        }
    }
}
