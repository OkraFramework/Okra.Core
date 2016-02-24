using Okra.Activation;
using Okra.DependencyInjection;
using Okra.Mvvm;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

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
                    // TODO : Create a root window & root shell
                    var appHost = new WindowAppHost(); // app.ApplicationServices.GetService<WindowAppHost>();
                    var appShell = new DefaultAppShell(new ViewFactory()); // app.ApplicationServices.GetService<DefaultAppShell>();
                    appHost.SetShell(appShell);
                    appShell.NavigateTo(new PageInfo(pageName, arguments));
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
