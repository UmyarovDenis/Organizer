using System.Windows;

namespace AdminPanel.Services.Local
{
    internal sealed class MessageService : IMessageService<bool>
    {
        public void ShowMessage(string message)
        {
            MessageBox.Show(message, "Сообщение", MessageBoxButton.OK, MessageBoxImage.Information);
        }
        public void ShowExclamation(string exclamation)
        {
            MessageBox.Show(exclamation, "Предупреждение", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public void ShowError(string error)
        {
            MessageBox.Show(error, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        public bool ShowQuestion(string question)
        {
            var result = MessageBox.Show(question, "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question);

            if (result == MessageBoxResult.Yes || result == MessageBoxResult.OK)
                return true;
            else
                return false;
        }
    }
}
