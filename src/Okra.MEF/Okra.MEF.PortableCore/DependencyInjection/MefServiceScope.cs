using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Composition;

namespace Okra.MEF.DependencyInjection
{
    internal class MefServiceScope : IServiceScope
    {
        private IServiceProvider _serviceProvider;
        private Export<CompositionContext> _scopeExport;

        public MefServiceScope(Export<CompositionContext> scopeExport)
        {
            _scopeExport = scopeExport;
            _serviceProvider = new MefServiceProvider(scopeExport.Value);
        }

        public IServiceProvider ServiceProvider
        {
            get
            {
                return _serviceProvider;
            }
        }

        public void Dispose()
        {
            _scopeExport.Dispose();
        }
    }
}
