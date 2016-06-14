using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.DependencyInjection
{
    public interface IServiceInjector<T>
    {
        bool HasValue { get; }
        T Service { get; set; }
    }
}
