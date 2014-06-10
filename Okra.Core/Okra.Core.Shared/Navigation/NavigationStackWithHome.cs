using System;
using System.Collections.Generic;
using System.Text;

namespace Okra.Navigation
{
    public class NavigationStackWithHome : NavigationStack
    {
        // *** Properties ***

        public override bool CanGoBack
        {
            get
            {
                return base.Count > 1;
            }
        }
    }
}
