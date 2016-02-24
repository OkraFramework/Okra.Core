using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Mvvm
{
    public interface IViewFactory
    {
        object CreateView(PageInfo page);
    }
}
