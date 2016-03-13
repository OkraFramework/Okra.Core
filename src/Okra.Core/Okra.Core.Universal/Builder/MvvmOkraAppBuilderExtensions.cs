using Okra.Activation;
using Okra.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.DependencyInjection;
using Windows.ApplicationModel.Activation;
using Okra.Navigation;

namespace Okra.Builder
{
    public static class MvvmOkraAppBuilderExtensions
    {
        public static IOkraAppBuilder UseStartPage(this IOkraAppBuilder app, string pageName, object arguments)
        {
            return app.Use((context, next) =>
            {
                var request = context.ActivationRequest as UniversalAppActivationRequest;

                if (request != null && request.EventArgs.Kind == ActivationKind.Launch)
                {
                    // TODO : Create a new app container
                    // TODO : Also restore navigation stack (or whole root app container?)
                    var appHost = app.ApplicationServices.GetRequiredService<WindowAppHost>();
                    var appShell = app.ApplicationServices.GetRequiredService<DefaultAppShell>();
                    appHost.SetShell(appShell);

                    var navigationManager = app.ApplicationServices.GetRequiredService<INavigationManager>();
                    navigationManager.NavigateTo(new PageInfo(pageName, arguments));

                    return Task.FromResult<bool>(true);
                }
                else
                {
                    return next();
                }
            });
        }
    }
}
