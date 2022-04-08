using System;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectFileMetadata : ProjectItemMetadata
    {
        public ProjectFileMetadata(ProjectFolderMetadata parent, string name, FileBuildAction buildAction)
            : base(parent, name)
        {
            BuildAction = buildAction;
        }

        public FileBuildAction BuildAction { get; internal set; }
    }
}
