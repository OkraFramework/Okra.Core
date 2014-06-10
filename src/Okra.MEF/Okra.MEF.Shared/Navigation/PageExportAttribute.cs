using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    [MetadataAttribute]
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = false)]
    public class PageExportAttribute : ExportAttribute
    {
        // *** Constructors ***

        public PageExportAttribute(Type type)
            : this(Okra.Navigation.PageName.FromType(type))
        {
        }

        public PageExportAttribute(string pageName)
            : base("OkraPage", typeof(object))
        {
            this.PageName = pageName;
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
