using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF.DependencyInjection
{
    [Export(typeof(IServiceScopeFactory))]
    [Shared]
    internal class MefServiceScopeFactory : IServiceScopeFactory
    {
        ExportFactory<CompositionContext> _scopeFactory;

        [ImportingConstructor]
        public MefServiceScopeFactory([SharingBoundary(MefServiceProvider.SHARING_BOUNDARY)]ExportFactory<CompositionContext> scopeFactory)
        {
            _scopeFactory = scopeFactory;
        }

        public IServiceScope CreateScope()
        {
            var scopeExport = _scopeFactory.CreateExport();
            return new MefServiceScope(scopeExport);
        }
    }
}
