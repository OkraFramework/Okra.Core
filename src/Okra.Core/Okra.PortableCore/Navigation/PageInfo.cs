using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Okra.Navigation
{
    public class PageInfo
    {
        // *** Constructors ***

        public PageInfo(string pageName, object arguments)
        {
            this.PageName = pageName;
            this.Arguments = arguments;
        }

        // *** Properties ***

        public object Arguments { get; }
        public string PageName { get; }
    }
}