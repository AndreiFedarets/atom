using System;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectMetadata
    {
        public ProjectMetadata(ProjectFileCollectionMetadata files)
        {
            Files = files;
        }

        public ProjectFileCollectionMetadata Files { get; private set; }
    }
}
