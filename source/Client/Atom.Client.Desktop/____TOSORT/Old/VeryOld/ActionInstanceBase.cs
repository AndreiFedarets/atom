using Atom.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom
{
    internal abstract class ActionInstanceBase<T> : IActionInstance where T : IActionType
    {
        private readonly IProjectMetadata _solutionMetadata;
        private readonly Lazy<T> _actionType;
        private readonly Lazy<IArgumentCollection> _arguments;

        protected ActionInstanceBase(Guid uid, Guid actionTypeUid, IProjectMetadata metadata)
        {
            Uid = uid;
            TypeUid = actionTypeUid;
            _solutionMetadata = metadata;
            _actionType = new Lazy<T>(GetActionType);
            _arguments = new Lazy<IArgumentCollection>(GetArguments);
        }

        protected ActionInstanceBase(ActionInstanceBaseInfo info, IProjectMetadata metadata)
            : this(info.ActionUid, info.ActionTypeUid, metadata)
        {
        }

        public Guid Uid { get; private set; }

        public Guid TypeUid { get; private set; }

        protected T ActionType
        {
            get { return _actionType.Value; }
        }

        public string Message
        {
            get { return ActionType.Message; }
        }

        public IArgumentCollection Arguments
        {
            get { return _arguments.Value; }
        }

        private IArgumentCollection GetArguments()
        {
            IEnumerable<IArgument> arguments = ActionType.Parameters.Select(x => (IArgument)new Argument(x));
            return new ArgumentCollection(arguments);
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

        private T GetActionType()
        {
            T actionType = (T)_solutionMetadata.ResolveActionType(TypeUid);
            return actionType;
        }
    }
}
