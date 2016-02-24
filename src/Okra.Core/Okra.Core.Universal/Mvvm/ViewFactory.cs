using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Mvvm
{
    public class ViewFactory : IViewFactory
    {
        public object CreateView(PageInfo page)
        {
            return Activator.CreateInstance(Type.GetType(page.PageName));
        }
    }
}
