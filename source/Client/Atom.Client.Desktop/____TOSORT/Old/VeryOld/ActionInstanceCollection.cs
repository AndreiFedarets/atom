using Atom.Serialization;

namespace Atom
{
    internal class ActionInstanceCollection : ReadOnlyCollection<IActionInstance>, IActionInstanceCollection, ILockable, ISerializable<ActionInstanceInfoCollection>
    {
        private readonly IProjectMetadata _metadata;
        private volatile bool _locked;

        public ActionInstanceCollection(IProjectMetadata metadata)
        {
            _locked = false;
            _metadata = metadata;
        }

        internal ActionInstanceCollection(ActionInstanceInfoCollection info, SerializationContext context)
            : this(context.Metadata)
        {
            foreach (ActionInstanceInfo actionInstanceInfo in info)
            {
                IActionInstance instance = _metadata.CreateActionInstance(actionInstanceInfo.ActionUid, actionInstanceInfo.ActionTypeUid);
                AddInternal(instance);
            }
        }

        public virtual IActionInstance Insert(int index, IActionType actionType)
        {
            if (_locked)
            {
                throw Fail.Design.TempException();
            }
            IActionInstance instance = _metadata.CreateActionInstance(actionType);
            InsertInternal(index, instance);
            return instance;
        }

        public virtual IActionInstance Add(IActionType actionType)
        {
            if (_locked)
            {
                throw Fail.Design.TempException();
            }
            IActionInstance instance = _metadata.CreateActionInstance(actionType);
            AddInternal(instance);
            return instance;
        }

        void ILockable.Lock()
        {
            _locked = true;
        }

        void ILockable.Unlock()
        {
            _locked = false;
        }

        ActionInstanceInfoCollection ISerializable<ActionInstanceInfoCollection>.Serialize()
        {
            ActionInstanceInfoCollection actionInstanceInfos = new ActionInstanceInfoCollection();
            foreach (IActionInstance actionInstance in this)
            {
                ActionInstanceInfo actionInstanceInfo = actionInstance.Serailize<ActionInstanceInfo>();
                actionInstanceInfos.Add(actionInstanceInfo);
            }
            return actionInstanceInfos;
        }
    }
}
