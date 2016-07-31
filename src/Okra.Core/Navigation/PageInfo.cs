using Okra.Helpers;
using System;

namespace Okra.Navigation
{
    public class PageInfo
    {
        // *** Constructors ***

        public PageInfo(string pageName, object arguments)
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            this.PageName = pageName;
            this.Arguments = arguments;
        }

        // *** Properties ***

        public object Arguments { get; }
        public string PageName { get; }

        // *** Methods ***

        public override string ToString() => Arguments == null ? PageName : $"{PageName}({Arguments})";
    }
}