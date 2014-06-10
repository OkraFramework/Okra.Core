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
    public class ViewModelExportAttribute : ExportAttribute
    {
        // *** Constructors ***

        public ViewModelExportAttribute(Type type)
            : this(Okra.Navigation.PageName.FromType(type))
        {
        }

        public ViewModelExportAttribute(string pageName)
            : base("OkraViewModel", typeof(object))
        {
            this.PageName = pageName;
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
