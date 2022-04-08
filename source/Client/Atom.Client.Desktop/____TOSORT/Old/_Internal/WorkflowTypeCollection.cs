using System;
using System.Linq;

namespace Atom
{
    internal sealed class WorkflowTypeCollection : ReadOnlyCollection<IWorkflowType>, IWorkflowTypeCollection
    {
        private readonly IAssemblyManager _assemblyManager;
        private readonly IActionTreeWalker _treeWalker;

        public WorkflowTypeCollection(IAssembly assembly, IAssemblyManager assemblyManager, IActionTreeWalker treeWalker)
        {
            Assembly = assembly;
            _assemblyManager = assemblyManager;
            _treeWalker = treeWalker;
        }

        public IAssembly Assembly { get; private set; }

        public IWorkflowType Create()
        {
            Guid uid = Guid.NewGuid();
            string message = string.Empty;
            ReferenceMetadata reference = Assembly.GetReferenceMetadata();
            WorkflowTypeMetadata metadata = new WorkflowTypeMetadata(uid, reference, message, Enumerable.Empty<ActionInstanceMetadata>());
            WorkflowType workflowType = new WorkflowType(metadata, Assembly, _assemblyManager, _treeWalker);
            AddInternal(workflowType);
            return workflowType;
        }

        //internal WorkflowTypeCollection(WorkflowTypeInfoCollection info, SerializationContext context)
        //    : this(context.TreeWalker, context.Metadata)
        //{
        //    foreach (WorkflowTypeInfo workflowTypeInfo in info)
        //    {
        //        WorkflowType workflowType = new WorkflowType(workflowTypeInfo, context);
        //        AddInternal(workflowType);
        //    }
        //}

        //WorkflowTypeInfoCollection ISerializable<WorkflowTypeInfoCollection>.Serialize()
        //{
        //    throw new NotImplementedException();
        //}
    }
}
