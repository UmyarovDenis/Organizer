using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ContractLibrary.Proxies
{
    public interface IProxyExecutor<TServiceContract> where TServiceContract : class
    {
        void Execute(Action<TServiceContract> action);
        Task ExecuteAsync(Action<TServiceContract> action);
        TResult GetServiceResult<TResult>(Func<TServiceContract, TResult> executor);
        Task<TResult> GetServiceResultAsync<TResult>(Func<TServiceContract, TResult> executor);
    }
}
