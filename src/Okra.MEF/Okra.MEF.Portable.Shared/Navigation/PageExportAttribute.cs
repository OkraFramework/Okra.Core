using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Okra.Navigation
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class PageExportAttribute : ExportAttribute
    {
        // *** Constructors ***

        public PageExportAttribute(Type type)
            : base("OkraPage", typeof(object))
        {
            if (type == null)
                throw new ArgumentNullException("type");

            this.PageName = Okra.Navigation.PageName.FromType(type);
        }

        public PageExportAttribute(string pageName)
            : base("OkraPage", typeof(object))
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), "pageName");

            this.PageName = pageName;
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
