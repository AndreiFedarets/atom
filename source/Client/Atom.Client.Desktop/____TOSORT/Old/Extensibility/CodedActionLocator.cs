using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Atom.Extensibility
{
    internal sealed class CodedActionLocator : IAssemblyLoader
    {
        private readonly IReflectionFacade _reflection;

        public CodedActionLocator(IReflectionFacade reflection)
        {
            _reflection = reflection;
        }

        public IEnumerable<IActionType> LoadActions(IActionAssembly assembly)
        {
            List<IActionType> actions = new List<IActionType>();
            foreach (Type type in assembly.Assembly.GetTypes())
            {
                actions.AddRange(LoadFromType(assembly, type));
            }
            return actions;
        }

        public IEnumerable<IActionType> LoadFromType(IActionAssembly actionAssembly, Type type)
        {
            bool staticType = type.IsAbstract && type.IsSealed;
            if (!staticType)
            {
                return Enumerable.Empty<IActionType>();
            }
            List<IActionType> actions = new List<IActionType>();
            foreach (MethodInfo methodInfo in type.GetMethods())
            {
                ActionMethodAttribute attribute = _reflection.GetActionMethodAttribute(methodInfo);
                if (attribute != null)
                {
                    CodedActionType action = new CodedActionType(attribute.Uid, attribute.Message, methodInfo, _reflection);
                    actions.Add(action);
                }
            }
            return actions;
        }
    }
}
