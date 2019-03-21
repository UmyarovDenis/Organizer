using System.ServiceModel;

namespace ContractLibrary.Authorization
{
    public interface IAuthorizationCallback
    {
        [OperationContract]
        void Message(string message);
    }
}
