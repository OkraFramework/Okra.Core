using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public interface IServiceInjector<T>
    {
        T Service { get; set; }
    }
}
