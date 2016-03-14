using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace Okra.Tests.Helpers
{
    public class UITestMethodAttribute : FactAttribute
    {
        public UITestMethodAttribute()
        {
            Skip = "Temporary placeholder for tests that need to run on a UI thread";
        }
    }
}
