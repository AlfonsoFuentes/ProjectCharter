using Toolbelt.Blazor;
using Web.Services.Identities.Accounts;

namespace Web.Services.Interceptor
{
    public interface IHttpInterceptorManager : IManagetAuth
    {
        void RegisterEvent();

        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

        void DisposeEvent();
    }
}