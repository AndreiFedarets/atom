using System;

namespace Atom.Design
{
    [Serializable]
    public sealed class ProjectFileMetadata
    {
        public ProjectFileMetadata(string include, string link, FileBuildAction buildAction, string generator)
        {
            BuildAction = buildAction;
            Include = include;
            Generator = generator;
            Link = link;
        }

        public string Include { get; private set; }

        public string Link { get; private set; }

        public string Generator { get; private set; }

        public FileBuildAction BuildAction { get; private set; }
    }
}
