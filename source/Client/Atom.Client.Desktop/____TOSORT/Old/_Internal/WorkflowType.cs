using System;
using System.Collections.Generic;
using System.Linq;

namespace Atom
{
    internal sealed class WorkflowType : ReadOnlyCollection<IActionInstance>, IWorkflowType, ILockable
    {
        private readonly ResetableLazy<IParameterCollection> _parameters;
        private readonly IAssemblyManager _assemblyManager;
        private readonly WorkflowTypeMetadata _metadata;
        private readonly IActionTreeWalker _treeWalker;
        private volatile bool _locked;

        public WorkflowType(WorkflowTypeMetadata metadata, IAssembly assembly, IAssemblyManager assemblyManager, IActionTreeWalker treeWalker)
        {
            _metadata = metadata;
            Assembly = assembly;
            _assemblyManager = assemblyManager;
            _treeWalker = treeWalker;
            _locked = true;
            _parameters = new ResetableLazy<IParameterCollection>(GetParameters);
            foreach (ActionInstanceMetadata actionInstanceMetadata in metadata)
            {
                IActionInstance instance = new ActionInstance(actionInstanceMetadata, _assemblyManager);
                AddInternal(instance);
            }
        }

        public IParameterCollection Parameters
        {
            get { return _parameters.Value; }
        }

        public IAssembly Assembly { get; private set; }

        public WorkflowTypeMetadata GetMetadata()
        {
            Guid uid = _metadata.Uid;
            ReferenceMetadata reference = Assembly.GetReferenceMetadata();
            string message = _metadata.Message;
            IEnumerable<ActionInstanceMetadata> actions = this.Select(x => x.GetMetadata());
            WorkflowTypeMetadata metadata = new WorkflowTypeMetadata(uid, reference, message, actions);
            return metadata;
        }

        ActionTypeMetadata IActionType.GetMetadata()
        {
            return GetMetadata();
        }

        private IParameterCollection GetParameters()
        {
            IList<IArgument> arguments = _treeWalker.FindArgumentsWithBinding<IExternalValueBinding>(this);
            IEnumerable<IParameter> parameters = arguments.Select(x => x.GetParameter());
            return new ParameterCollection(parameters);
        }

        public IActionInstance Insert(int index, IActionType actionType)
        {
            if (_locked)
            {
                throw Fail.Design.TempException();
            }
            IActionInstance instance = CreateActionInstance(actionType);
            InsertInternal(index, instance);
            _parameters.Reset();
            return instance;
        }

        public IActionInstance Add(IActionType actionType)
        {
            if (_locked)
            {
                throw Fail.Design.TempException();
            }
            IActionInstance instance = CreateActionInstance(actionType);
            AddInternal(instance);
            _parameters.Reset();
            return instance;
        }

        public void Remove(Guid actionInstanceUid)
        {
            if (_locked)
            {
                throw Fail.Design.TempException();
            }
            IActionInstance instance = Find(x => x.Uid == actionInstanceUid);
            if (instance != null)
            {
                RemoveInternal(instance);
                _parameters.Reset();
            }
        }

        void ILockable.Lock()
        {
            _locked = true;
        }

        void ILockable.Unlock()
        {
            if (!_locked)
            {
                throw Fail.Design.TempException();
            }
            _locked = false;
        }

        private IActionInstance CreateActionInstance(IActionType actionType)
        {
            ActionTypeMetadata typeMetadata = actionType.GetMetadata();
            Guid uid = Guid.NewGuid();
            ActionInstanceMetadata instanceMetadata = new ActionInstanceMetadata(uid, typeMetadata);
            IActionInstance instance = new ActionInstance(instanceMetadata, _assemblyManager);
            return instance;
        }

        //WorkflowTypeInfo ISerializable<WorkflowTypeInfo>.Serialize()
        //{
        //    WorkflowTypeInfo workflowTypeInfo = new WorkflowTypeInfo();
        //    workflowTypeInfo.Uid = Uid;
        //    workflowTypeInfo.Message = Message;
        //    workflowTypeInfo.Actions = new ActionInstanceInfoCollection();
        //    foreach (IActionInstance actionInstance in this)
        //    {
        //        ActionInstanceInfo actionInstanceInfo = actionInstance.Serailize<ActionInstanceInfo>();
        //        workflowTypeInfo.Actions.Add(actionInstanceInfo);
        //    }
        //    return workflowTypeInfo;
        //}
    }
}
