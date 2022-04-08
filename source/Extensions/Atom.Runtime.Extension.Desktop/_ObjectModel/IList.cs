using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.Runtime.Extension.Desktop.ObjectModel
{
    public interface IList : IControl
    {
        bool CanSelectionMultiple { get; }
        bool IsSelectionRequered { get; }
        string Value { get; }
        bool IsReadonly { get; }
    }
}
