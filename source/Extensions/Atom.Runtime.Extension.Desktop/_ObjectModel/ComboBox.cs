using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop.ObjectModel
{
    public class ComboBox : Control, IComboBox
    {

        public ComboBox(params ControlProperty[] properties)
            : base(properties, ControlType.ComboBox)
        {
        }


        public string Value => Pattern.Current.Value;

        public bool IsReadonly => Pattern.Current.IsReadOnly;


        private ValuePattern Pattern => (ValuePattern) Element.GetCurrentPattern(ValuePattern.Pattern);
    }
}
