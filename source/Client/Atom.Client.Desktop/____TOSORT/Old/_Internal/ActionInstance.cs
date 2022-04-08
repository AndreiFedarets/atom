using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom
{
    internal sealed class ActionInstance : IActionInstance
    {
        private readonly ActionInstanceMetadata _metadata;
        private readonly IAssemblyManager _assemblyManager;
        private readonly Lazy<IActionType> _actionType;
        private readonly Lazy<IArgumentCollection> _arguments;

        public ActionInstance(ActionInstanceMetadata metadata, IAssemblyManager assemblyManager)
        {
            _metadata = metadata;
            _assemblyManager = assemblyManager;
            _actionType = new Lazy<IActionType>(GetActionType);
            _arguments = new Lazy<IArgumentCollection>(GetArguments);
        }

        public Guid Uid
        {
            get { return _metadata.Uid; }
        }

        internal IActionType ActionType
        {
            get { return _actionType.Value; }
        }

        public IArgumentCollection Arguments
        {
            get { return _arguments.Value; }
        }

        public ActionInstanceMetadata GetMetadata()
        {
            ActionTypeMetadata typeMetadata = _metadata.TypeMetadata;
            if (_actionType.IsValueCreated)
            {
                typeMetadata = _actionType.Value.GetMetadata();
            }
            ActionInstanceMetadata metadata = new ActionInstanceMetadata(_metadata.Uid, typeMetadata);
            return metadata;
        }

        object IDataSource.GetValue(string valueName)
        {
            IArgument argument = Arguments[valueName];
            if (argument == null)
            {
                throw Fail.Execution.TempException();
            }
            IExactValueBinding valueBinding = argument.ValueBinding as IExactValueBinding;
            if (valueBinding == null)
            {
                throw Fail.Execution.TempException();
            }
            return valueBinding.Value;
        }

        private IActionType GetActionType()
        {
            ReferenceMetadata reference = _metadata.TypeMetadata.AssemblyReference;
            IAssembly assembly = _assemblyManager.LoadAssembly(reference);
            IActionType actionType = assembly.FindAction(_metadata.TypeMetadata.Uid);
            return actionType;
        }

        private IArgumentCollection GetArguments()
        {
            IEnumerable<IArgument> arguments = ActionType.Parameters.Select(x => (IArgument)new Argument(x));
            return new ArgumentCollection(arguments);
        }
    }
}
