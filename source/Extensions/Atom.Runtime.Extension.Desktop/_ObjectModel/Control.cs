using System.Collections.Generic;
using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop
{
    public abstract class Control : IControl
    {
        private readonly ControlProperty[] _properties;
        private readonly ControlType _controlType;
        private AutomationElement _element;
        private IControl _parent;

        protected Control(ControlProperty[] properties, ControlType controlType)
        {
            _properties = properties;
            _controlType = controlType;
        }

        public AutomationElement Element
        {
            get
            {
                if (_element == null)
                {
                    _element = FindElement();
                }
                return _element;
            }
        }

        public IEnumerable<ControlProperty> Properties
        {
            get { return _properties; }
        }

        public bool IsDisplayed
        {
            get { return !Element.Current.IsOffscreen; }
        }

        public bool IsEnabled
        {
            get { return Element.Current.IsEnabled; }
        }

        public void AttachTo(IControl parent)
        {
            _parent = parent;
        }
        
        private AutomationElement FindElement()
        {
            AutomationElement parentElement = _parent == null ? AutomationElement.RootElement : _parent.Element;
            Condition condition = GetCondition();
            AutomationElement element = parentElement.FindFirst(TreeScope.Subtree, condition);
            return element;
        }

        private Condition GetCondition()
        {
            List<Condition> conditions = new List<Condition>();
            conditions.Add(new PropertyCondition(AutomationElement.ControlTypeProperty, _controlType));
            foreach (ControlProperty property in _properties)
            {
                AutomationProperty automationProperty = AutomationProperty.LookupById(property.Id);
                conditions.Add(new PropertyCondition(automationProperty, property.Value));
            }
            return new AndCondition(conditions.ToArray());
        }
    }
}
