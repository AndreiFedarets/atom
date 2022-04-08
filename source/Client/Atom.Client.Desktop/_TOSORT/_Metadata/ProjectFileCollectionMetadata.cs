using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectFileCollectionMetadata : ReadOnlyCollection<ProjectFileMetadata>
    {
        public ProjectFileCollectionMetadata(IEnumerable<ProjectFileMetadata> items)
            : base(new List<ProjectFileMetadata>(items))
        {
        }
    }
}
