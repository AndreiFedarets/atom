using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Atom.Runtime.Extension.Desktop.ObjectModel
{
    public interface IComboBox : IControl
    {
        string Value { get; }
        bool IsReadonly { get; }
    }
}
