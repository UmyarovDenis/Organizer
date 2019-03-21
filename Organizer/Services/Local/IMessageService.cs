namespace Organizer.Services.Local
{
    interface IMessageService<out TResult>
    {
        void ShowMessage(string message);
        void ShowExclamation(string exclamation);
        void ShowError(string error);
        TResult ShowQuestion(string question);
    }
}
