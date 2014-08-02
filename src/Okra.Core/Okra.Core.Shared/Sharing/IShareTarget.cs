using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public interface IShareTarget
    {
        // *** Methods ***

        void Activate(IShareOperation shareOperation);
    }
}
