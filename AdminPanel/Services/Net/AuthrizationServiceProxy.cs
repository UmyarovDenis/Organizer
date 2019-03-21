using ContractLibrary.Authorization;
using ContractLibrary.Proxies;

namespace AdminPanel.Services.Net
{
    internal sealed class AuthorizationServiceProxy : SingleServiceProxy<IAuthorizationContract>
    {
        public AuthorizationServiceProxy() : base("AuthServiceHttp")
        { }
    }
}
