using ContractLibrary.Notifyer;
using ContractLibrary.Proxies;
using System;
using System.ServiceModel;

namespace DataTransferService.Proxies
{
    internal sealed class NotifyProxyService : SingleServiceProxy<INotifyNode>
    {
        public NotifyProxyService()
            : base(new Uri("net.tcp://localhost:7030/INotifyNode"), new NetTcpBinding())
        {

        }
    }
}
