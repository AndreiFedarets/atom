using System.Windows.Automation;

namespace Atom.Runtime.Extension.Desktop.ObjectModel
{
    public class List : Control, IList // Сомневаюсь пока что нужен ли он т к особо смысла в нем нет ... экшенов тут нет особо ... только проперти ... чуть что потом можно будет удалить
    {
        public List(params ControlProperty[] properties)
            : base(properties, ControlType.List)
        {
        }

        public bool CanSelectionMultiple => PatternSelect.Current.CanSelectMultiple;

        public bool IsSelectionRequered => PatternSelect.Current.IsSelectionRequired;

        public string Value => PatternValue.Current.Value;

        public bool IsReadonly => PatternValue.Current.IsReadOnly;



        private SelectionPattern PatternSelect =>(SelectionPattern) Element.GetCurrentPattern(SelectionPattern.Pattern);

        private ValuePattern PatternValue => (ValuePattern) Element.GetCurrentPattern(ValuePattern.Pattern);
    }
}
