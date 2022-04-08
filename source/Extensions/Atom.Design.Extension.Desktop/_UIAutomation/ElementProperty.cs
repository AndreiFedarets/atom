using System.Windows.Automation;

namespace Atom.Design.Extension.Desktop
{
    public sealed class ElementProperty
    {
        private readonly AutomationProperty _automationProperty;
        private readonly AutomationElement _automationElement;

        public ElementProperty(AutomationProperty automationProperty, AutomationElement automationElement)
        {
            _automationProperty = automationProperty;
            _automationElement = automationElement;
            DisplayName = Automation.PropertyName(_automationProperty);
            //InitializeValue();
        }

        public int Id
        {
            get { return _automationProperty.Id; }
        }

        public string DisplayName { get; private set; }
        
        public object Value
        {
            get
            {
                //InitializeValue();
                //return _value;
                return _automationElement.GetCurrentPropertyValue(_automationProperty);
            }
        }
        
        public override bool Equals(object obj)
        {
            if (ReferenceEquals(this, obj))
            {
                return true;
            }
            ElementProperty other = obj as ElementProperty;
            if (other == null)
            {
                return false;
            }
            bool equals = _automationProperty.Equals(other._automationProperty);
            equals = equals && _automationElement.Equals(other._automationElement);
            return equals;
        }

        public bool Equals(ElementProperty other)
        {
            return Equals((object)other);
        }

        public override int GetHashCode()
        {
            unchecked
            {
                int hash = _automationProperty.GetHashCode();
                hash = hash ^ _automationElement.GetHashCode();
                return hash;
            }
        }

        //private void InitializeValue()
        //{
        //    if (!_valueInitialized)
        //    {
        //        _valueInitialized = true;
        //        _value = _automationElement.GetCurrentPropertyValue(_automationProperty);
        //    }
        //}
    }
}
