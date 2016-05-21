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
    public class OkraAppBuilderFixture
    {
        [Fact]
        public void Constructor_SetsApplicationServicesProperty()
        {
            IServiceProvider serviceProvider = new MockServiceProvider();
            OkraAppBuilder appBuilder = new OkraAppBuilder(serviceProvider);

            Assert.Equal(serviceProvider, appBuilder.ApplicationServices);
        }

        [Fact]
        public void Constructor_ThrowsException_IfServiceProviderIsNull()
        {
            Assert.Throws<ArgumentNullException>("serviceProvider", () => new OkraAppBuilder(null));
        }

        [Fact]
        public async Task Build_BuildsTheOverallPipeline()
        {
            var serviceProvider = new MockServiceProvider();
            var appBuilder = new OkraAppBuilder(serviceProvider);

            var middlewareCallList = new List<string>();
            var middlewareCallContextList = new List<AppLaunchContext>();

            appBuilder.Use(next =>
                            {
                                return context =>
                                {
                                    middlewareCallList.Add("First");
                                    middlewareCallContextList.Add(context);
                                    return next(context);
                                };
                            });

            appBuilder.Use(next =>
                            {
                                return context =>
                                {
                                    middlewareCallList.Add("Second");
                                    middlewareCallContextList.Add(context);
                                    return Task.FromResult<bool>(true);
                                };
                            });

            appBuilder.Use(next =>
                            {
                                return context =>
                                {
                                    middlewareCallList.Add("Third");
                                    middlewareCallContextList.Add(context);
                                    return next(context);
                                };
                            });

            var appContext = new MockAppLaunchContext();
            var pipeline = appBuilder.Build();
            await pipeline(appContext);

            Assert.Equal(new string[] { "First", "Second" }, middlewareCallList);
            Assert.Equal(new AppLaunchContext[] { appContext, appContext }, middlewareCallContextList);
        }

        [Fact]
        public async Task Build_BuildsAPipelineThatHasNoFinalStep()
        {
            var serviceProvider = new MockServiceProvider();
            var appBuilder = new OkraAppBuilder(serviceProvider);

            var middlewareCallList = new List<string>();
            var middlewareCallContextList = new List<AppLaunchContext>();

            appBuilder.Use(next =>
            {
                return context =>
                {
                    middlewareCallList.Add("First");
                    middlewareCallContextList.Add(context);
                    return next(context);
                };
            });

            appBuilder.Use(next =>
            {
                return context =>
                {
                    middlewareCallList.Add("Second");
                    middlewareCallContextList.Add(context);
                    return next(context);
                };
            });

            var appContext = new MockAppLaunchContext();
            var pipeline = appBuilder.Build();
            await pipeline(appContext);

            Assert.Equal(new string[] { "First", "Second" }, middlewareCallList);
            Assert.Equal(new AppLaunchContext[] { appContext, appContext }, middlewareCallContextList);
        }

        [Fact]
        public void Use_ThrowsException_IfFunctionIsNull()
        {
            IServiceProvider serviceProvider = new MockServiceProvider();
            OkraAppBuilder appBuilder = new OkraAppBuilder(serviceProvider);

            Assert.Throws<ArgumentNullException>("middleware", () => appBuilder.Use(null));
        }
    }
}
