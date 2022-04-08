using Atom.Design.Reflection.Metadata;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Atom.Design
{
    public sealed class ArgumentCollection : ReadOnlyCollection<Argument>
    {
        public ArgumentCollection(InvokeInstruction instruction)
            : base(Initialize(instruction))
        {
        }

        public Argument this[ParameterReference parameter]
        {
            get { return Items.FirstOrDefault(x => x.Parameter == parameter); }
        }

        public Argument this[string parameterName]
        {
            get { return Items.FirstOrDefault(x => string.Equals(x.Parameter.Name, parameterName, StringComparison.Ordinal)); }
        }

        private static IList<Argument> Initialize(InvokeInstruction instruction)
        {
            List<Argument> collection = new List<Argument>();
            foreach (ParameterReference parameter in instruction.Method.Reference.Parameters)
            {
                Argument argument;
                switch (parameter.Direction)
                {
                    case ParameterDirection.Input:
                        argument = new InputArgument(parameter);
                        break;
                    case ParameterDirection.Output:
                        argument = new OutputArgument(parameter);
                        break;
                    default:
                        throw new NotSupportedException();
                }
                collection.Add(argument);
            }
            return collection;
        }
    }
}
