using System;

namespace Atom.Design
{
    [Serializable]
    public abstract class ProjectItemMetadata
    {
        protected ProjectItemMetadata(ProjectFolderMetadata parent, string name)
        {
            Name = name;
            Parent = parent;
        }

        public string Name { get; internal set; }

        public ProjectFolderMetadata Parent { get; private set; }

        internal static bool NamesEquals(string name1, string name2)
        {
            return string.Equals(name1, name2, StringComparison.OrdinalIgnoreCase);
        }
    }
}
