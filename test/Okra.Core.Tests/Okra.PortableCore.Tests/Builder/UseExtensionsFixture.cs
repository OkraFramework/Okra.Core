using Okra.Lifetime;
using Okra.Builder;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Builder
{
    public class UseExtensionsFixture
    {
        [Fact]
        public async Task Use_AddsMiddlewareUsingInlineSyntax()
        {
            IServiceProvider serviceProvider = new MockServiceProvider();
            OkraAppBuilder app = new OkraAppBuilder(serviceProvider);

            var middlewareCallList = new List<string>();
            var middlewareCallContextList = new List<AppLaunchContext>();

            app.Use((context, next) =>
                {
                    middlewareCallList.Add("Inline");
                    middlewareCallContextList.Add(context);
                    return next();
                });

            app.Use(next =>
            {
                return context =>
                {
                    middlewareCallList.Add("Next");
                    middlewareCallContextList.Add(context);
                    return next(context);
                };
            });

            var appContext = new MockAppLaunchContext();
            var pipeline = app.Build();
            await pipeline(appContext);

            Assert.Equal(new string[] { "Inline", "Next" }, middlewareCallList);
            Assert.Equal(new AppLaunchContext[] { appContext, appContext }, middlewareCallContextList);
        }
    }
}
