using System.Collections.Generic;
using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public sealed class ElementPatternCollection : LazyReadOnlyCollection<ElementPattern>
    {
        private readonly AutomationElement _automationElement;

        public ElementPatternCollection(AutomationElement automationElement)
        {
            _automationElement = automationElement;
        }

        protected override IList<ElementPattern> Initialize()
        {
            List<ElementPattern> collection = new List<ElementPattern>();
            foreach (AutomationPattern automationPattern in _automationElement.GetSupportedPatterns())
            {
                ElementPattern pattern = new ElementPattern(_automationElement, automationPattern);
                collection.Add(pattern);
            }
            return collection;
        }
    }
}
