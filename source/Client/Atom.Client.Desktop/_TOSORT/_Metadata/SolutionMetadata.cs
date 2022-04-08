using System;

namespace Atom.Design
{
    [Serializable]
    public sealed class SolutionMetadata
    {
        public SolutionMetadata(string name, ProjectCollectionMetadata projects)
        {
            Name = name;
            Projects = projects;
        }

        public string Name { get; internal set; }

        public ProjectCollectionMetadata Projects { get; private set; }
    }
}
