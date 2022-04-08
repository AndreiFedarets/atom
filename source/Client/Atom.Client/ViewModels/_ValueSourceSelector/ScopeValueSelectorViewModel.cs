using Atom.Design;
using Atom.Design.Reflection;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Client.ViewModels
{
    public sealed class ScopeValueSelectorViewModel : ValueSourceSelectorViewModel
    {
        private readonly InputArgument _inputArgument;
        private readonly List<IValueSource> _availableValues;

        public ScopeValueSelectorViewModel(InputArgumentEditorViewModel parentViewModel, Method method, InputArgument inputArgument)
            : base(parentViewModel)
        {
            DisplayName = "Current Scope";
            _inputArgument = inputArgument;
            //_availableValues = DesignerHelpers.FindAvailableValueSources(method, inputArgument);
        }
        
        public IEnumerable<IValueSource> AvailableValues
        {
            get { return _availableValues; }
        }

        internal override void SelectCurrentValueSource()
        {
            //SelectedValue = AvailableValues.FirstOrDefault(x => x.Equals(_inputArgument.Source));
        }
    }
}
