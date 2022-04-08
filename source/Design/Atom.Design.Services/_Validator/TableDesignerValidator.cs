using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public sealed class DataTableDesignerValidator : IDesignerValidator
    {
        public bool Validate(IObjectDesigner designer)
        {
            Table table = designer as Table;
            if (table == null)
            {
                return false;
            }
            List<string> valueNames = new List<string>();
            foreach (TableValue tableValueDesigner in table)
            {
                if (valueNames.Contains(tableValueDesigner.ValueName))
                {
                    return false;
                }
                valueNames.Add(tableValueDesigner.ValueName);
            }
            return true;
        }
    }
}
