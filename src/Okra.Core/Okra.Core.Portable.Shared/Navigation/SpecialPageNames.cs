using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public static class SpecialPageNames
    {
        public const string Home = "Home";

#if NETFX_CORE
        public const string Search = "Search";
        public const string ShareTarget = "ShareTarget";
#endif
    }
}
