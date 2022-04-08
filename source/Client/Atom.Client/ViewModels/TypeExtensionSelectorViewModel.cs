using Atom.Design.Services;
using Layex.ViewModels;
using System.Collections.Generic;

namespace Atom.Client.ViewModels
{
    public sealed class TypeExtensionSelectorViewModel : DialogViewModel
    {
        public TypeExtensionSelectorViewModel(ITypeService typeService)
        {
            Extensions = typeService.GetExtensions();
        }

        public IEnumerable<ITypeExtension> Extensions { get; private set; }

        public ITypeExtension Extension { get; set; }

        public void Submit()
        {
            TryClose(true);
        }

        public void Cancel()
        {
            TryClose(false);
        }
    }
}
