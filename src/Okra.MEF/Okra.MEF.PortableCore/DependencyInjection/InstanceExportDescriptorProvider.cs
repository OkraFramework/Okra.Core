using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF.DependencyInjection
{
    // Based upon https://mef.codeplex.com/SourceControl/latest#oob/demo/Microsoft.Composition.Demos.ExtendedPartTypes/Extension/InstanceExportDescriptorProvider.cs
    // TODO: A new export descriptior provider for each type - could I have a single instance that does a table lookup?

    internal class InstanceExportDescriptorProvider : ExportDescriptorProvider
    {
        ServiceDescriptor _serviceDescriptor;

        public InstanceExportDescriptorProvider(ServiceDescriptor serviceDescriptor)
        {
            _serviceDescriptor = serviceDescriptor;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != _serviceDescriptor.ServiceType)
                return NoExportDescriptors;

            return new[] { new ExportDescriptorPromise(contract, _serviceDescriptor.ImplementationInstance.ToString(), true, NoDependencies, _ =>
                    ExportDescriptor.Create((c, o) => _serviceDescriptor.ImplementationInstance, NoMetadata)) };
        }
    }
}
