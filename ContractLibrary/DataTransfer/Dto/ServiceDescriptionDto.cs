using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.Serialization;

namespace ContractLibrary.DataTransfer.Dto
{
    [DataContract]
    public class ServiceDescriptionDto : BaseDto, INotifyPropertyChanged
    {
        private TimeSpan _workingHours;

        public ServiceDescriptionDto()
        {
            _workingHours = new TimeSpan();
        }
        [DataMember] public string ServiceType { get; set; }
        [DataMember] public string ServiceName { get; set; }
        [DataMember] public string State { get; set; }
        [DataMember] public DateTime StartDate { get; set; }
        [DataMember] public DateTime StopDate { get; set; }
        [DataMember] public TimeSpan WorkingHours
        {
            get { return _workingHours; }
            set
            {
                _workingHours = value;
                RaisePropertyChanged(nameof(WorkingHours));
            }
        }
        [DataMember] public string Address { get; set; }
        [DataMember] public string Binding { get; set; }
        [DataMember] public int ListenBacklog { get; set; }
        [DataMember] public string OpenTimeout { get; set; }
        [DataMember] public string CloseTimeout { get; set; }
        [DataMember] public string SendTimeout { get; set; }
        [DataMember] public string ReceiveTimeout { get; set; }
        [DataMember] public long MaxBufferPoolSize { get; set; }
        [DataMember] public bool IsStarted { get; set; }

        private void RaisePropertyChanged([CallerMemberName]string propertyName = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        public event PropertyChangedEventHandler PropertyChanged;
    }
}
