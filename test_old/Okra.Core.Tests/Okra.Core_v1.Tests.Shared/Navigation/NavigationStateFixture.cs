using Okra.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Okra.Tests.Navigation
{
    public class NavigationStateFixture
    {
        // *** Property Tests ***

        [Fact]
        public void NavigationStack_IsEmptyCollection()
        {
            NavigationState state = new NavigationState();

            Assert.NotNull(state.NavigationStack);
            Assert.Equal(0, state.NavigationStack.Count);
        }
    }
}
