using System.ComponentModel;

namespace AdminPanel.Enums
{
    internal enum ServerStatus
    {
        [Description("Ожидание")]
        Idle = 0,
        [Description("Сервер запущен")]
        Active = 1,
        [Description("Сервер остановлен")]
        Stoped = 2
    }
}
