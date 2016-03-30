using Microsoft.Extensions.DependencyInjection;
using Okra.MEF.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF.Util
{
    public static class CompositeActivatorExtensions
    {
        public static CompositeActivator ApplyServiceLifetime(this CompositeActivator activator, ServiceLifetime serviceLifetime)
        {
            if (serviceLifetime == ServiceLifetime.Transient)
            {
                return activator;
            }
            else
            {
                var sharingId = LifetimeContext.AllocateSharingId();
                return (c, o) =>
                {
                    // Find the root composition scope.
                    var sharingBoundary = serviceLifetime == ServiceLifetime.Scoped ? MefServiceProvider.SHARING_BOUNDARY : null;
                    var scope = c.FindContextWithin(sharingBoundary);
                    if (scope == c)
                    {
                        // We're already in the root scope, create the instance
                        return scope.GetOrCreate(sharingId, o, activator);
                    }
                    else
                    {
                        // Composition is moving up the hierarchy of scopes; run
                        // a new operation in the root scope.
                        return CompositionOperation.Run(scope, (c1, o1) => c1.GetOrCreate(sharingId, o1, activator));
                    }
                };
            };
        }
    }
}
