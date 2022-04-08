using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectCollectionMetadata : ReadOnlyCollection<ProjectMetadata>
    {
        public ProjectCollectionMetadata(IEnumerable<ProjectMetadata> items)
            : base(new List<ProjectMetadata>(items))
        {
        }
    }
}
