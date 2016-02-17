using Okra.Activation;
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
            var middlewareCallContextList = new List<AppActivationContext>();

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

            var appContext = new MockAppActivationContext();
            var pipeline = app.Build();
            await pipeline(appContext);

            Assert.Equal(new string[] { "Inline", "Next" }, middlewareCallList);
            Assert.Equal(new AppActivationContext[] { appContext, appContext }, middlewareCallContextList);
        }
    }
}
