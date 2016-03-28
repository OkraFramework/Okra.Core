using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Composition.Hosting.Core;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Okra.MEF.Util;

namespace Okra.MEF.DependencyInjection
{
    public class TypeExportDescriptorProvider : ExportDescriptorProvider
    {
        private Type _contractType;
        private Type _implementationType;
        private ServiceLifetime _lifetime;

        private static readonly MethodInfo s_getTypedDescriptorMethod = typeof(TypeExportDescriptorProvider).GetTypeInfo().GetDeclaredMethod(nameof(GetTypedDescriptor));

        public TypeExportDescriptorProvider(Type contractType, Type implementationType, ServiceLifetime lifetime)
        {
            this._contractType = contractType;
            this._implementationType = implementationType;
            this._lifetime = lifetime;
        }

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (contract.ContractType != _contractType)
                return NoExportDescriptors;

            var implementationContract = contract.ChangeType(_implementationType);

            var getDescriptorMethod = s_getTypedDescriptorMethod.MakeGenericMethod(_contractType);
            var getDescriptorDelegate = getDescriptorMethod.CreateStaticDelegate<Func<CompositionContract, CompositionContract, DependencyAccessor, object>>();

            return new[] { (ExportDescriptorPromise)getDescriptorDelegate(contract, implementationContract, descriptorAccessor) };
        }

        private static ExportDescriptorPromise GetTypedDescriptor<TElement>(CompositionContract contract, CompositionContract implementationContract, DependencyAccessor definitionAccessor)
        {
            var longestConstructor = GetLongestComposableConstructor(implementationContract, definitionAccessor);
            var constructor = longestConstructor.Item1;
            var dependencies = longestConstructor.Item2;

            return new ExportDescriptorPromise(
                 contract,
                 typeof(TElement).Name,
                 false,
                 () => dependencies,
                 ds =>
                 {
                     var parameterActivators = ds.Select(d => d.Target.GetDescriptor().Activator).ToArray();

                     return ExportDescriptor.Create((c, o) =>
                     {
                         var parameters = parameterActivators.Select(activator => CompositionOperation.Run(c, activator)).ToArray();
                         var export = constructor.Invoke(parameters);
                         return export;
                     }, NoMetadata);
                 });
        }

        private static Tuple<ConstructorInfo, IEnumerable<CompositionDependency>> GetLongestComposableConstructor(CompositionContract implementationContract, DependencyAccessor definitionAccessor)
        {
            var constructors = implementationContract.ContractType.GetTypeInfo().DeclaredConstructors
                                .Where(c => c.IsPublic)
                                .OrderByDescending(c => c.GetParameters().Length);

            foreach (var constructor in constructors)
            {
                IEnumerable<CompositionDependency> constructorDependencies;

                if (TryResolveConstructorDependencies(constructor, definitionAccessor, out constructorDependencies))
                    return Tuple.Create(constructor, constructorDependencies);
            }

            throw new NotImplementedException();
        }

        private static bool TryResolveConstructorDependencies(ConstructorInfo constructor, DependencyAccessor definitionAccessor, out IEnumerable<CompositionDependency> dependencies)
        {
            var parameters = constructor.GetParameters();
            var dependencyArray = new CompositionDependency[parameters.Length];

            for (int i = 0; i < parameters.Length; i++)
            {
                CompositionDependency dependency;
                var parameterContract = new CompositionContract(parameters[i].ParameterType);

                if (definitionAccessor.TryResolveOptionalDependency("parameter", parameterContract, true, out dependency))
                {
                    dependencyArray[i] = dependency;
                }
                else
                {
                    dependencies = null;
                    return false;
                }
            }

            dependencies = dependencyArray;
            return true;
        }
    }
}
