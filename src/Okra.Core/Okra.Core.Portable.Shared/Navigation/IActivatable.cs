using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Navigation
{
    public interface IActivatable
    {
        // *** Methods ***

        void Activate(PageInfo pageInfo);
        void SaveState(PageInfo pageInfo);
    }
}
