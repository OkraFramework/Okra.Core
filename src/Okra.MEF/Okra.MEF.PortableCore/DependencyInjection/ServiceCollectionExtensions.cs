using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition;
using System.Composition.Convention;
using System.Composition.Hosting;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Okra.MEF.DependencyInjection
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceProvider BuildServiceProvider(this IServiceCollection services)
        {
            var rootContainer = CreateContainer(services);
            var scopeFactory = rootContainer.GetExport<IServiceScopeFactory>();
            var rootScope = scopeFactory.CreateScope();

            return rootScope.ServiceProvider;
        }

        // *** Private Methods ***

        private static CompositionContext CreateContainer(IEnumerable<ServiceDescriptor> serviceDescriptors)
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
                            containerConfiguration.WithProvider(new FactoryExportDescriptorProvider(descriptor.ImplementationFactory, descriptor.ServiceType, null, null, true));
                        else if (descriptor.ImplementationType != null)
                            AddSingletonPart(containerConfiguration, descriptor.ImplementationType, descriptor.ServiceType);
                        else
                            throw new NotImplementedException();
                        break;
                    case ServiceLifetime.Transient:
                        if (descriptor.ImplementationFactory != null)
                            containerConfiguration.WithProvider(new FactoryExportDescriptorProvider(descriptor.ImplementationFactory, descriptor.ServiceType, null, null, false));
                        else if (descriptor.ImplementationType != null)
                            AddTransientPart(containerConfiguration, descriptor.ImplementationType, descriptor.ServiceType);
                        else
                            throw new NotImplementedException();
                        break;
                    case ServiceLifetime.Scoped:
                        if (descriptor.ImplementationFactory != null)
                            throw new NotImplementedException();
                        else if (descriptor.ImplementationType != null)
                            AddScopedPart(containerConfiguration, descriptor.ImplementationType, descriptor.ServiceType);
                        else
                            throw new NotImplementedException();
                        break;
                    default:
                        throw new NotImplementedException();
                }
            }

            containerConfiguration.WithPart<MefServiceProvider>();
            containerConfiguration.WithPart<MefServiceScopeFactory>();

            containerConfiguration.WithProvider(new EnumerableExportDescriptorProvider());

            return containerConfiguration.CreateContainer();
        }

        private static void AddSingletonPart(ContainerConfiguration containerConfiguration, Type implementationType, Type serviceType)
        {
            ConventionBuilder conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForType(implementationType)
                             .Export(e => e.AsContractType(serviceType))
                             .SelectConstructor(GetLongestConstructor)
                             .Shared();

            containerConfiguration.WithPart(implementationType, conventionBuilder);
        }

        private static void AddTransientPart(ContainerConfiguration containerConfiguration, Type implementationType, Type serviceType)
        {
            ConventionBuilder conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForType(implementationType)
                             .Export(e => e.AsContractType(serviceType))
                             .SelectConstructor(GetLongestConstructor);

            containerConfiguration.WithPart(implementationType, conventionBuilder);
        }

        private static void AddScopedPart(ContainerConfiguration containerConfiguration, Type implementationType, Type serviceType)
        {
            ConventionBuilder conventionBuilder = new ConventionBuilder();

            conventionBuilder.ForType(implementationType)
                             .Export(e => e.AsContractType(serviceType))
                             .SelectConstructor(GetLongestConstructor)
                             .Shared(MefServiceProvider.SHARING_BOUNDARY);

            containerConfiguration.WithPart(implementationType, conventionBuilder);
        }

        private static ConstructorInfo GetLongestConstructor(IEnumerable<ConstructorInfo> constructors)
        {
            return constructors.OrderByDescending(c => c.GetParameters().Length).First();
        }
    }
}
