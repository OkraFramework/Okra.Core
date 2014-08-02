using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Sharing
{
    public interface IShareSourceManager
    {
        // *** Properties ***

        string DefaultFailureText { get; set; }

        // *** Methods ***

        void ShowShareUI();
    }
}
