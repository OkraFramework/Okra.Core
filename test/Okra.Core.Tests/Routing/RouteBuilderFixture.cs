using Okra.Routing;
using Okra.Tests.Mocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Routing
{
    public class RouteBuilderFixture
    {
        [Fact]
        public void Constructor_SetsApplicationServicesProperty()
        {
            IServiceProvider serviceProvider = new MockServiceProvider();
            RouteBuilder routeBuilder = new RouteBuilder(serviceProvider);

            Assert.Equal(serviceProvider, routeBuilder.ApplicationServices);
        }

        [Fact]
        public void Constructor_ThrowsException_IfServiceProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceProvider", () => new RouteBuilder(null));
        }

        [Fact]
        public async Task Build_BuildsTheOverallPipeline()
        {
            var serviceProvider = new MockServiceProvider();
            var routeBuilder = new RouteBuilder(serviceProvider);

            var routerCallList = new List<string>();
            var routerCallContextList = new List<RouteContext>();

            routeBuilder.AddRouter(next =>
            {
                return async context =>
                {
                    routerCallList.Add("First");
                    routerCallContextList.Add(context);
                    return string.Concat("Alpha ", await next(context));
                };
            });

            routeBuilder.AddRouter(next =>
            {
                return context =>
                {
                    routerCallList.Add("Second");
                    routerCallContextList.Add(context);
                    return Task.FromResult<object>("Beta");
                };
            });

            routeBuilder.AddRouter(next =>
            {
                return context =>
                {
                    routerCallList.Add("Third");
                    routerCallContextList.Add(context);
                    return next(context);
                };
            });

            var routeContext = new RouteContext();
            var router = routeBuilder.Build();
            var result = await router(routeContext);

            Assert.Equal(new string[] { "First", "Second" }, routerCallList);
            Assert.Equal(new RouteContext[] { routeContext, routeContext }, routerCallContextList);
            Assert.Equal("Alpha Beta", result);
        }

        [Fact]
        public async Task Build_BuildsAPipelineThatHasNoFinalStep()
        {
            var serviceProvider = new MockServiceProvider();
            var routeBuilder = new RouteBuilder(serviceProvider);

            var routerCallList = new List<string>();
            var routerCallContextList = new List<RouteContext>();

            routeBuilder.AddRouter(next =>
            {
                return context =>
                {
                    routerCallList.Add("First");
                    routerCallContextList.Add(context);
                    return next(context);
                };
            });

            routeBuilder.AddRouter(next =>
            {
                return context =>
                {
                    routerCallList.Add("Second");
                    routerCallContextList.Add(context);
                    return next(context);
                };
            });

            var routeContext = new RouteContext();
            var router = routeBuilder.Build();
            var result = await router(routeContext);

            Assert.Equal(new string[] { "First", "Second" }, routerCallList);
            Assert.Equal(new RouteContext[] { routeContext, routeContext }, routerCallContextList);
            Assert.Null(result);
        }

        [Fact]
        public void Use_ThrowsException_IfFunctionIsNull()
        {
            IServiceProvider serviceProvider = new MockServiceProvider();
            RouteBuilder routeBuilder = new RouteBuilder(serviceProvider);

            Assert.Throws<ArgumentNullException>("router", () => routeBuilder.AddRouter(null));
        }
    }
}
