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
    class EnumerableExportDescriptorProvider : ExportDescriptorProvider
    {
        private static readonly MethodInfo s_getEnumerableDescriptorMethod = typeof(EnumerableExportDescriptorProvider).GetTypeInfo().GetDeclaredMethod(nameof(GetEnumerableDescriptor));

        public override IEnumerable<ExportDescriptorPromise> GetExportDescriptors(CompositionContract contract, DependencyAccessor descriptorAccessor)
        {
            if (!contract.ContractType.IsConstructedGenericType || contract.ContractType.GetGenericTypeDefinition() != typeof(IEnumerable<>))
                return NoExportDescriptors;

            var elementType = contract.ContractType.GenericTypeArguments[0];
            var elementContract = contract.ChangeType(elementType);

            var getDescriptorMethod = s_getEnumerableDescriptorMethod.MakeGenericMethod(elementType);
            var getDescriptorDelegate = getDescriptorMethod.CreateStaticDelegate<Func<CompositionContract, CompositionContract, DependencyAccessor, object>>();

            return new[] { (ExportDescriptorPromise)getDescriptorDelegate(contract, elementContract, descriptorAccessor) };
        }

        private static ExportDescriptorPromise GetEnumerableDescriptor<TElement>(CompositionContract enumerableContract, CompositionContract elementContract, DependencyAccessor definitionAccessor)
        {
            return new ExportDescriptorPromise(
                 enumerableContract,
                 typeof(TElement[]).Name,
                 false,
                 () => definitionAccessor.ResolveDependencies("item", elementContract, true),
                 d =>
                 {
                     var dependentDescriptors = d
                         .Select(el => el.Target.GetDescriptor())
                         .ToArray();

                     return ExportDescriptor.Create((c, o) => dependentDescriptors.Select(e => (TElement)e.Activator(c, o)).ToArray(), NoMetadata);
                 });
        }
    }
}
