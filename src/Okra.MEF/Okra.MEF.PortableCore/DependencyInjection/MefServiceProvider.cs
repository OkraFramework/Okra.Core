using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reflection;
using System.Composition.Hosting;
using System.Composition.Convention;
using Microsoft.Extensions.DependencyInjection;

namespace Okra.MEF.DependencyInjection
{
    internal class MefServiceProvider : IServiceProvider
    {
        // *** Fields ***

        private CompositionHost _compositionHost;

        // *** Constructors ***

        public MefServiceProvider(IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            CreateContainer(serviceDescriptors);
        }

        // *** Methods ***

        public object GetService(Type serviceType)
        {
            // Try to compose the service using the default MEF container
            
            object export;

            if (_compositionHost.TryGetExport(serviceType, out export))
            {
                return export;
            }
            else
            {
                // If this fails and the service type is IEnumerable<T> then use GetExports(...)

                TypeInfo serviceTypeInfo = serviceType.GetTypeInfo();

                if (serviceTypeInfo.IsGenericType && serviceTypeInfo.GetGenericTypeDefinition() == typeof(IEnumerable<>))
                {
                    var serviceElementType = serviceTypeInfo.GenericTypeArguments[0];
                    var exportArray = _compositionHost.GetExports(serviceElementType);
                    return exportArray;
                }

                // Return null if the service type could not be found

                return null;
            }
        }

        // *** Private Methods ***

        private void CreateContainer(IEnumerable<ServiceDescriptor> serviceDescriptors)
        {
            var containerConfiguration = new ContainerConfiguration();

            foreach (var descriptor in serviceDescriptors)
            {
                switch (descriptor.Lifetime)
                {
                    case ServiceLifetime.Singleton:
                        if (descriptor.ImplementationInstance != null)
                            containerConfiguration.WithProvider(new InstanceExportDescriptorProvider(descriptor.ImplementationInstance, descriptor.ServiceType, null, null));
                        else if (descriptor.ImplementationFactory != null)
                            containerConfiguration.WithProvider(new DelegateExportDescriptorProvider(() => descriptor.ImplementationFactory(this), descriptor.ServiceType, null, null, true));
                        else if (descriptor.ImplementationType != null)
                            AddSingletonPart(containerConfiguration, descriptor.ImplementationType, descriptor.ServiceType);
                        else
                            throw new NotImplementedException();
                        break;
                    case ServiceLifetime.Transient:
                        if (descriptor.ImplementationFactory != null)
                            containerConfiguration.WithProvider(new DelegateExportDescriptorProvider(() => descriptor.ImplementationFactory(this), descriptor.ServiceType, null, null, false));
                        else if (descriptor.ImplementationType != null)
                            AddTransientPart(containerConfiguration, descriptor.ImplementationType, descriptor.ServiceType);
                        else
                            throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            _compositionHost = containerConfiguration.CreateContainer();
        }

        private void AddSingletonPart(ContainerConfiguration containerConfiguration, Type implementationType, Type serviceType)
        {
            ConventionBuilder conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForType(implementationType)
                             .Export(e => e.AsContractType(serviceType))
                             .Shared();

            containerConfiguration.WithPart(implementationType, conventionBuilder);
        }

        private void AddTransientPart(ContainerConfiguration containerConfiguration, Type implementationType, Type serviceType)
        {
            ConventionBuilder conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForType(implementationType)
                             .Export(e => e.AsContractType(serviceType));

            containerConfiguration.WithPart(implementationType, conventionBuilder);
        }
    }
}
