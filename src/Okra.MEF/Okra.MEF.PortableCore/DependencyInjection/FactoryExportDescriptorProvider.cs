using Microsoft.Extensions.DependencyInjection;
using Okra.MEF.Util;
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

            _activator = constructor.ApplyServiceLifetime(_serviceDescriptor.Lifetime);
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != _serviceDescriptor.ServiceType)
                return NoExportDescriptors;

            return new[] { new ExportDescriptorPromise(contract, "factory delegate", true, NoDependencies, _ => ExportDescriptor.Create(_activator, NoMetadata)) };
        }

    }
}
