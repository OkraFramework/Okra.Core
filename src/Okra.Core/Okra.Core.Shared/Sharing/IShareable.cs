using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public interface IShareable
    {
        Task ShareRequested(IShareRequest shareRequest);
    }
}
