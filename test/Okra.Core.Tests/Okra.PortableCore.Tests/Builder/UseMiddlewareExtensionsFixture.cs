using Okra.Builder;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;
using Okra.Activation;
using Okra.Tests.Mocks;

namespace Okra.Tests.Builder
{
    public class UseMiddlewareExtensionsFixture
    {
        [Fact]
        public void UseMiddleware_WithNullApp_ThrowsException()
        {
            MockMiddlewareOptions options = new MockMiddlewareOptions();

            var e = Assert.Throws<ArgumentNullException>(() => UseMiddlewareExtensions.UseMiddleware<MockMiddleware, MockMiddlewareOptions>(null, options));

            Assert.Equal("app", e.ParamName);
        }

        [Fact]
        public async Task UseMiddleware_AddsMiddleware()
        {
            IServiceProvider serviceProvider = new MockServiceProvider()
                                                    .With<MockMiddleware>(new MockMiddleware());
            OkraAppBuilder app = new OkraAppBuilder(serviceProvider);

            var middlewareCallList = new List<string>();
            var middlewareCallContextList = new List<AppActivationContext>();
            var options = new MockMiddlewareOptions(middlewareCallList, middlewareCallContextList);

            app.UseMiddleware<MockMiddleware, MockMiddlewareOptions>(options);

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

            Assert.Equal(new string[] { "Middleware", "Next" }, middlewareCallList);
            Assert.Equal(new AppActivationContext[] { appContext, appContext }, middlewareCallContextList);
        }

        // *** Private sub-classes ***

        private class MockMiddleware : IMiddleware<MockMiddlewareOptions>
        {
            ActivationDelegate _next;
            MockMiddlewareOptions _options;

            public void Configure(ActivationDelegate next, MockMiddlewareOptions options)
            {
                _next = next;
                _options = options;
            }

            public Task Invoke(AppActivationContext context)
            {
                _options.MiddlewareCallList.Add("Middleware");
                _options.MiddlewareCallContextList.Add(context);
                return _next(context);
            }
        }

        private class MockMiddlewareOptions
        {
            public MockMiddlewareOptions()
            {
            }

            public MockMiddlewareOptions(List<string> middlewareCallList, List<AppActivationContext> middlewareCallContextList)
            {
                this.MiddlewareCallList = middlewareCallList;
                this.MiddlewareCallContextList = middlewareCallContextList;
            }

            public List<AppActivationContext> MiddlewareCallContextList { get; }
            public List<string> MiddlewareCallList { get; }
        }
    }
}
