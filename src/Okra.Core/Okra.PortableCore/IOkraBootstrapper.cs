using Okra.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra
{
    public interface IOkraBootstrapper
    {
        Task Activate(IAppActivationRequest activationRequest);
        void Initialize();
    }
}
