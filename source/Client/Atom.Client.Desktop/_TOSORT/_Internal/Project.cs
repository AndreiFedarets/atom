using System;

namespace Atom.Design
{
    internal sealed class Project : IProject
    {
        private readonly Lazy<IProjectFolder> _projectFolder;

        public Project(Guid uid, string name, string path, ISolution solution)
        {
            Uid = uid;
            Name = name;
            Path = path;
            Solution = solution;
            ShadowAssembly = new ProjectAssembly(this);
            References = new AssemblyReferenceCollection();
            Settings = new ProjectSettings();
            //_projectFolder = new Lazy<IProjectFolder>(GetProjectFolder);
        }

        public Guid Uid { get; private set; }

        //TODO: Implement renaming
        public string Name { get; set; }

        public string Path { get; private set; }

        public IAssembly ShadowAssembly { get; private set; }

        public ISolution Solution { get; private set; }

        public ProjectSettings Settings { get; private set; }

        public IAssemblyReferenceCollection References { get; private set; }

        public IProjectFolder ProjectFolder
        {
            get { return _projectFolder.Value; }
        }

        public event EventHandler<NameChangedEventArgs> NameChanged;

        //private IProjectFolder GetProjectFolder()
        //{
        //    ProjectFolder = new ProjectFolder(null, );
        //}

        //ProjectInfo ISerializable<ProjectInfo>.Serialize()
        //{
        //    ProjectInfo projectInfo = new ProjectInfo();
        //    projectInfo.Uid = Uid;
        //    projectInfo.Name = Name;
        //    projectInfo.Workflows = Workflows.Serailize<WorkflowTypeInfoCollection>();
        //    return projectInfo;
        //}
    }
}
