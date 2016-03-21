using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Composition.Hosting;
using System.Composition.Convention;
using Microsoft.Extensions.DependencyInjection;
using System.Composition;

namespace Okra.MEF.DependencyInjection
{
    [Export(typeof(IServiceProvider))]
    internal class MefServiceProvider : IServiceProvider
    {
        public const string SHARING_BOUNDARY = "Scope";

        // *** Fields ***

        private CompositionContext _compositionContext;

        // *** Constructors ***

        [ImportingConstructor]
        public MefServiceProvider(CompositionContext compositionContext)
        {
            _compositionContext = compositionContext;
        }

        // *** Methods ***

        public object GetService(Type serviceType)
        {
            // Try to compose the service using the default MEF container

            var exports = _compositionContext.GetExports(serviceType).ToList();

            if (exports.Count > 0)
            {
                return exports.Last();
            }
            else
            {
                return null;
            }
        }
    }
}
