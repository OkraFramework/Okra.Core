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
            : base("OkraViewModel", typeof(object))
        {
            throw new NotImplementedException();
        }

        public ViewModelExportAttribute(string pageName)
            : base("OkraViewModel", typeof(object))
        {
            throw new NotImplementedException();
        }

        // *** Properties ***

        public string PageName { get; private set; }
    }
}
