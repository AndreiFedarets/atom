using System;
using System.Collections;
using System.Collections.Generic;
using Atom.Serialization;

namespace Atom
{
    internal sealed class WorkflowInstance : ActionInstanceBase<IWorkflowType>, IWorkflowInstance, ISerializable<WorkflowInstanceInfo>
    {
        public WorkflowInstance(Guid uid, Guid actionTypeUid, IProjectMetadata metadata)
            : base(uid, actionTypeUid, metadata)
        {
        }

        internal WorkflowInstance(WorkflowInstanceInfo info, IProjectMetadata metadata)
            : base(info, metadata)
        {
        }

        public IEnumerator<IActionInstance> GetEnumerator()
        {
            return ActionType.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        WorkflowInstanceInfo ISerializable<WorkflowInstanceInfo>.Serialize()
        {
            WorkflowInstanceInfo workflowInstanceInfo = new WorkflowInstanceInfo();
            workflowInstanceInfo.ActionTypeUid = ActionType.Uid;
            return workflowInstanceInfo;
        }
    }
}
