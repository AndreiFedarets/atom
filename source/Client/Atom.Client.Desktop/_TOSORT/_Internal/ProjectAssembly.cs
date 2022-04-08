using System;
using System.Collections.Generic;
using Atom.Metadata;

namespace Atom.Design
{
    internal sealed class ProjectAssembly : IAssembly
    {
        private readonly IProject _project;

        public ProjectAssembly(IProject project)
        {
            _project = project;
        }

        public AssemblyMetadata Metadata
        {
            get { throw new NotImplementedException(); }
        }

        public IAction this[Guid actionUid]
        {
            get { throw new NotImplementedException(); }
        }
        
        //public AssemblyInfo AssemblyInfo
        //{
        //    get
        //    {
        //        string name = _project.Name;
        //        string fileFullName = string.Empty;
        //        Version version = _project.Settings.Version;
        //        AssemblyType type = AssemblyType.Managed;
        //        AssemblyInfo assemblyInfo = new AssemblyInfo(name, fileFullName, version, type);
        //        return assemblyInfo;
        //    }
        //}

        //public IActionType FindAction(Guid actionTypeUid)
        //{
        //    IActionType actionType = _project.Workflows.FirstOrDefault(x => x.GetMetadata().Uid == actionTypeUid);
        //    return actionType;
        //}

        //public IActionTypeCollection EnumerateActions()
        //{
        //    IDictionary<Guid, IActionType> dictionary = _project.Workflows.ToDictionary(x => x.GetMetadata().Uid, x => (IActionType)x);
        //    IActionTypeCollection collection = new ActionTypeCollection(dictionary);
        //    return collection;
        //}

        //public AssemblyReference GetReference()
        //{
        //    string assemblyName = _project.Name;
        //    string projectVersion = _project.Settings.Version.ToString();
        //    AssemblyReference reference = new AssemblyReference(assemblyName, AssemblyType.Managed, projectVersion);
        //    return reference;
        //}
    }
}
