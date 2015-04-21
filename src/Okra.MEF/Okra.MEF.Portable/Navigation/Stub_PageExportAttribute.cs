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
            : base("OkraPage", typeof(object))
        {
            throw new NotImplementedException();
        }

        public PageExportAttribute(string pageName)
            : base("OkraPage", typeof(object))
        {
            throw new NotImplementedException();
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
