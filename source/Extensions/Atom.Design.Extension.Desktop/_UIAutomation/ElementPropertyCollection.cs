using System.Collections.Generic;
using System.Linq;
using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public sealed class ElementPropertyCollection : LazyReadOnlyCollection<ElementProperty>
    {
        private readonly AutomationElement _automationElement;

        public ElementPropertyCollection(AutomationElement automationElement)
        {
            _automationElement = automationElement;
        }

        public ElementProperty this[AutomationProperty automationProperty]
        {
            get { return this.First(x => x.Id == automationProperty.Id); }
        }

        public int ProcessId
        {
            get { return (int)this[AutomationElement.ProcessIdProperty].Value; }
        }

        public string AutomationId
        {
            get { return (string)this[AutomationElement.AutomationIdProperty].Value; }
        }

        public string Name
        {
            get { return (string)this[AutomationElement.NameProperty].Value; }
        }

        public string ClassName
        {
            get { return (string)this[AutomationElement.ClassNameProperty].Value; }
        }

        public ControlType ControlType
        {
            get { return (ControlType)this[AutomationElement.ControlTypeProperty].Value; }
        }

        public System.Windows.Rect BoundingRectangle
        {
            get { return (System.Windows.Rect)this[AutomationElement.BoundingRectangleProperty].Value; }
        }

        protected override IList<ElementProperty> Initialize()
        {
            List<ElementProperty> collection = new List<ElementProperty>();
            foreach (AutomationProperty automationProperty in _automationElement.GetSupportedProperties())
            {
                ElementProperty property = new ElementProperty(automationProperty, _automationElement);
                collection.Add(property);
            }
            collection.Sort((x, y) => string.Compare(x.DisplayName, y.DisplayName));
            return collection;
        }
    }
}
