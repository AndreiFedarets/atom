using System.Collections.Generic;
using System.Linq;

namespace Atom.Design.Services
{
    public abstract class MethodDesignerValidator : IDesignerValidator
    {
        public abstract bool Validate(IObjectDesigner designer);

        protected virtual bool Validate(Method method)
        {
            if (method == null)
            {
                return false;
            }
            List<string> valueNames = new List<string>();
            foreach (InvokeInstruction instance in method.Instructions.OfType<InvokeInstruction>())
            {
                foreach (OutputArgument outputArgument in instance.Arguments.OfType<OutputArgument>())
                {
                    if (valueNames.Contains(outputArgument.ValueName))
                    {
                        return false;
                    }
                    valueNames.Add(outputArgument.ValueName);
                }
                foreach (InputArgument inputArgument in instance.Arguments.OfType<InputArgument>())
                {
                    if (inputArgument.Value == null)
                    {
                        return false;
                    }
                }
            }
            return true;
        }
    }
}
