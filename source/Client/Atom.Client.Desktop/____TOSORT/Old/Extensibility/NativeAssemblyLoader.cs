using System.Reflection;

namespace Atom.Extensibility
{
    internal sealed class NativeAssemblyLoader : IAssemblyLoader
    {
        private readonly IReflectionFacade _reflection;

        public NativeAssemblyLoader(IReflectionFacade reflection)
        {
            _reflection = reflection;
        }

        public IAssembly LoadAssembly(string fileFullName)
        {
            Assembly assembly = _reflection.LoadAssembly(fileFullName);
            ActionAssemblyAttribute attribute = _reflection.GetActionAssemblyAttribute(assembly);
            if (attribute == null)
            {
                return null;
            }
            IAssembly managedAssembly = new NativeAssembly(assembly, _reflection);
            return managedAssembly;
        }

        public bool IsValidAssemblyFile(string fileFullName)
        {
            Assembly assembly = _reflection.LoadAssembly(fileFullName);
            ActionAssemblyAttribute attribute = _reflection.GetActionAssemblyAttribute(assembly);
            return attribute != null;
        }

        //public IEnumerable<IActionType> LoadFromType(IActionAssembly actionAssembly, Type type)
        //{
        //    bool staticType = type.IsAbstract && type.IsSealed;
        //    if (!staticType)
        //    {
        //        return Enumerable.Empty<IActionType>();
        //    }
        //    List<IActionType> actions = new List<IActionType>();
        //    foreach (MethodInfo methodInfo in type.GetMethods())
        //    {
        //        ActionMethodAttribute attribute = _reflection.GetActionMethodAttribute(methodInfo);
        //        if (attribute != null)
        //        {
        //            CodedActionType action = new CodedActionType(attribute.Uid, attribute.Message, methodInfo, _reflection);
        //            actions.Add(action);
        //        }
        //    }
        //    return actions;
        //}

    }
}
