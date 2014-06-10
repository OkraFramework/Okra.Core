using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.ApplicationModel.Activation;

namespace Okra.Services
{
    public interface IActivationHandler
    {
        // *** Methods ***

        Task<bool> Activate(IActivatedEventArgs activatedEventArgs);
    }
}
