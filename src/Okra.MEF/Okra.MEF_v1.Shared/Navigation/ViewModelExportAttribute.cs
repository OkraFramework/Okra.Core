using Okra.Helpers;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;

namespace Okra.Navigation
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class ViewModelExportAttribute : ExportAttribute
    {
        // *** Constructors ***

        public ViewModelExportAttribute(Type type)
            : base("OkraViewModel", typeof(object))
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            this.PageName = Okra.Navigation.PageName.FromType(type);
        }

        public ViewModelExportAttribute(string pageName)
            : base("OkraViewModel", typeof(object))
        {
            if (string.IsNullOrEmpty(pageName))
                throw new ArgumentException(ResourceHelper.GetErrorResource("Exception_ArgumentException_StringIsNullOrEmpty"), nameof(pageName));

            this.PageName = pageName;
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
