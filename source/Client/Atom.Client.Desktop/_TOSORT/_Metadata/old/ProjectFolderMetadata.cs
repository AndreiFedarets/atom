using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectFolderMetadata : ProjectItemMetadata, IReadOnlyCollection<ProjectItemMetadata>
    {
        private readonly List<ProjectItemMetadata> _items;

        public ProjectFolderMetadata(ProjectFolderMetadata parent, string name, IEnumerable<ProjectItemMetadata> items)
            : base(parent, name)
        {
            _items = new List<ProjectItemMetadata>(items);
        }

        public int Count
        {
            get { return _items.Count; }
        }

        public IEnumerable<ProjectFolderMetadata> Folders
        {
            get { return _items.OfType<ProjectFolderMetadata>(); }
        }

        public IEnumerable<ProjectFileMetadata> Files
        {
            get { return _items.OfType<ProjectFileMetadata>(); }
        }

        public IEnumerator<ProjectItemMetadata> GetEnumerator()
        {
            return _items.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        internal void Add(ProjectItemMetadata item)
        {
            _items.Add(item);
        }

        internal bool Remove(ProjectItemMetadata item)
        {
            return _items.Remove(item);
        }
    }
}
