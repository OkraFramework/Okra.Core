using Okra.Activation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.Builder
{
    public interface IMiddleware<TOptions>
    {
        void Configure(ActivationDelegate next, TOptions options);
        Task Invoke(AppActivationContext context);
    }
}
