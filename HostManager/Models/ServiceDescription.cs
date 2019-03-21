using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceProcess;
using System.Text;
using System.Threading.Tasks;

namespace HostManager.Models
{
    internal class ServiceDescription
    {
        private StringBuilder _stringBuilder;

        public string ServiceName { get; set; }
        public string MachineName { get; set; }
        public string Description { get; set; }
        public string ServiceStatus { get; set; }
        public string StartType { get; set; }
        public string ServiceState { get; set; }

        public ServiceDescription(ServiceController serviceController)
        {
            ServiceName = serviceController.ServiceName;
            MachineName = serviceController.MachineName;
            Description = serviceController.DisplayName;
            ServiceStatus = serviceController.Status.ToString();
            StartType = serviceController.StartType.ToString();
            ServiceState = "Установлена";
        }
        public override string ToString()
        {
            _stringBuilder = new StringBuilder();

            _stringBuilder.AppendFormat("Имя службы:  {0}\n", ServiceName);
            _stringBuilder.AppendFormat("Компьютер:   {0}\n", MachineName);
            _stringBuilder.AppendFormat("Описание:    {0}\n", Description);
            _stringBuilder.AppendFormat("Стаус:       {0}\n", ServiceStatus);
            _stringBuilder.AppendFormat("Тип запуска: {0}\n", StartType);
            _stringBuilder.AppendFormat("Состояние:   {0}\n", ServiceState);

            return _stringBuilder.ToString();
        }
    }
}
