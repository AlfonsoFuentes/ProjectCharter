using MudBlazorWeb.Services.Identities.Accounts;
using Toolbelt.Blazor;

namespace MudBlazorWeb.Services.Interceptor
{
    public interface IHttpInterceptorManager : IManagetAuth
    {
        void RegisterEvent();

        Task InterceptBeforeHttpAsync(object sender, HttpClientInterceptorEventArgs e);

        void DisposeEvent();
    }
}