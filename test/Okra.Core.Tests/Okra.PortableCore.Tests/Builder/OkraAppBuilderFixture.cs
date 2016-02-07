using Okra.Builder;
using Okra.MEF.Mocks;
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
    }
}
