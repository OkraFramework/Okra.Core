using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF.DependencyInjection
{
    // Based upon https://mef.codeplex.com/SourceControl/latest#oob/demo/Microsoft.Composition.Demos.ExtendedPartTypes/Extension/DelegateExportDescriptorProvider.cs

    internal class FactoryExportDescriptorProvider : ExportDescriptorProvider
    {
        ServiceDescriptor _serviceDescriptor;
        CompositeActivator _activator;

        public FactoryExportDescriptorProvider(ServiceDescriptor serviceDescriptor)
        {
            this._serviceDescriptor = serviceDescriptor;

            // Runs the factory method, validates the result and registers it for disposal if necessary.
            CompositeActivator constructor = (c, o) =>
            {
                var result = _serviceDescriptor.ImplementationFactory(c.GetExport<IServiceProvider>());
                if (result == null)
                    throw new InvalidOperationException("Delegate factory returned null.");

                if (result is IDisposable)
                    c.AddBoundInstance((IDisposable)result);

                return result;
            };

            if (_serviceDescriptor.Lifetime == ServiceLifetime.Transient)
            {
                _activator = constructor;
            }
            else
            {
                var sharingId = LifetimeContext.AllocateSharingId();
                _activator = (c, o) =>
                {
                    // Find the root composition scope.
                    var sharingBoundary = _serviceDescriptor.Lifetime == ServiceLifetime.Scoped ? MefServiceProvider.SHARING_BOUNDARY : null;
                    var scope = c.FindContextWithin(sharingBoundary);
                    if (scope == c)
                    {
                        // We're already in the root scope, create the instance
                        return scope.GetOrCreate(sharingId, o, constructor);
                    }
                    else
                    {
                        // Composition is moving up the hierarchy of scopes; run
                        // a new operation in the root scope.
                        return CompositionOperation.Run(scope, (c1, o1) => c1.GetOrCreate(sharingId, o1, constructor));
                    }
                };
            };
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != _serviceDescriptor.ServiceType)
                return NoExportDescriptors;

            return new[] { new ExportDescriptorPromise(contract, "factory delegate", true, NoDependencies, _ => ExportDescriptor.Create(_activator, NoMetadata)) };
        }

    }
}
